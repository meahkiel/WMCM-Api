using Application.Contacts;
using Application.Core;
using Application.DTO;
using Application.Interface;
using Core.Campaigns;
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

            
            

            private async Task<Activity> CreateSMSActivity(ActivityEntryDTO entry)
            {

                var contacts = await _context.Customers.GetContactByGroup(entry.ToGroup);
                ContactSerialize serialize = new ContactSerialize(contacts.ToList(), entry.ToGroup);
                List<string> mobileNos = serialize.ExtractSMS();
                var strmobileNos = string.Join(",", mobileNos.ToArray());
                
                string description = entry.ToGroup +
                                (!string.IsNullOrEmpty(entry.ToGroup) ? entry.ToGroup : "");
                
                await _activityService.CreateBulkSMS(mobileNos, entry.Body);

                return Activity.CreateSMSActivity(entry.Title, strmobileNos, entry.Body, DateTime.Now, null);

            } 

            private async Task<Activity> CreateEmailActivity(ActivityEntryDTO entry)
            {
                var contacts = await _context.Customers.GetContactByGroup(entry.ToGroup);
                ContactSerialize serialize = new ContactSerialize(contacts.ToList(), entry.To);
                
                List<string> emailAddress = serialize.ExtractEmail();
                var strEmailAddress = string.Join(",", emailAddress.ToArray());
                string description = $"Send Email to  {strEmailAddress}"; 
                              

                await _activityService.CreateEmail(emailAddress, entry.Subject, entry.Body);

                return Activity.CreateEmailActivity(entry.Subject,description,entry.Subject,entry.Body,DateTime.Now,strEmailAddress);

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
                    
                    Activity activity = null;
                    switch(request.Entry.Type.ToLower())
                    {
                        case "sms":
                            activity = await CreateSMSActivity(request.Entry);
                            break;
                        case "email":
                            activity = await CreateEmailActivity(request.Entry);
                            break;
                        case "web":
                           
                            break;
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
