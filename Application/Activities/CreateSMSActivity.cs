using Application.Contacts;
using Application.Core;
using Application.DTO;
using MediatR;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class CreateSMSActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ActivitySMSDTO SMSActivity { get; set; }

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
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {

                    var campaign = await _context.CampaignRepo.GetSingleCampaign(request.SMSActivity.CampaignId); 

                    //do not create if campaign is invalid
                    if (campaign == null)
                        throw new Exception("campaign doesn't exist");

                    var contacts = await _context.Customers.GetContactByGroup(request.SMSActivity.Group);
                    ContactSerialize serialize = new ContactSerialize(contacts.ToList(),request.SMSActivity.MobileNos);
                    List<string> mobileNos = serialize.ExtractSMS();
                    string description = request.SMSActivity.Group +
                                        (!string.IsNullOrEmpty(request.SMSActivity.MobileNos) ? request.SMSActivity.MobileNos : "");
                    
                    var activity = await _activityService.CreateBulkSMS(request.SMSActivity.Title,description,mobileNos,
                                              request.SMSActivity.Message,
                                              request.SMSActivity.DateToSend);

                    campaign.AddActivity(activity);
                    _context.CampaignRepo.Update(campaign);
                    
                    var result = await _context.SaveChangesAsync();
                    if(!result)
                    {
                        throw new Exception("Error on campaign");
                    }

                    return Result<Unit>.Success(Unit.Value);

                }catch (Exception ex)
                {
                    return Result<Unit>.Failure(ex.Message);
                }

            }
        }


    }
}
