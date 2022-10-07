using Application.Contacts;
using Application.Core;
using Application.DTO;
using Core.Campaigns;
using Core.Contacts;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class SendActivity
    {


        public class Command : IRequest<Result<Unit>>
        {
            public ActivityEntryDTO Entry { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IActivityService _activityService;
            private readonly UnitWrapper _context;


            public CommandHandler(IActivityService activityService,
                    UnitWrapper context)
            {
                _activityService = activityService;
                _context = context;

            }

            private async Task<Activity> CreateSMSActivity(IEnumerable<Contact> contacts,ActivityEntryDTO entry)
            {
                
                ContactSerialize serialize = new ContactSerialize(contacts.ToList(), entry.ToGroup);
                List<string> mobileNos = serialize.ExtractSMS();
                
                string description = entry.ToGroup +
                                (!string.IsNullOrEmpty(entry.ToGroup) ? entry.ToGroup : "");
                
                await _activityService.CreateBulkSMS(mobileNos, entry.Body);

                return Activity.CreateSMSActivity(entry.Title, entry.ToGroup, entry.Body, DateTime.Now, null);

            } 

            private async Task<Activity> CreateEmailActivity(IEnumerable<Contact> contacts,ActivityEntryDTO entry)
            {
                ContactSerialize serialize = new ContactSerialize(contacts.ToList(), entry.To);
                
                List<string> emailAddress = serialize.ExtractEmail();
                string description = $"Send Email to  {string.Join(",", emailAddress.ToArray())}"; 
                              

                await _activityService.CreateEmail(emailAddress, entry.Subject, entry.Body);

                return Activity.CreateEmailActivity(entry.Subject,description,entry.Subject,entry.Body,DateTime.Now, null);

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {

                    var campaign = await _context.CampaignRepo
                                               .GetSingleCampaign(request.Entry.CampaignId);
                    
                    //do not create if campaign is invalid
                    if (campaign == null)
                        throw new Exception("campaign doesn't exist");

                    var contacts = await _context.Customers.GetContactByGroup(request.Entry.ToGroup);
                    
                    Activity activity = null;

                    if(request.Entry.Type.ToLower() == "sms") {
                      activity = await CreateSMSActivity(contacts,request.Entry);
                    } 
                    else if (request.Entry.Type.ToLower() == "email") {
                        activity = await CreateEmailActivity(contacts, request.Entry);
                    }

                    if(activity != null)
                    {
                        campaign.AddActivity(activity);
                        _context.CampaignRepo.Update(campaign);
                        var result = await _context.SaveChangesAsync();
                        if (!result) {
                            throw new Exception("Error on campaign");
                        }
                    }
                    else {
                        throw new Exception("Undefined error");
                    }

                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }
            }
        }


    }
}
