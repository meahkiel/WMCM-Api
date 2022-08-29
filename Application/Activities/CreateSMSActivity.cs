using Application.Contacts;
using Application.Core;
using Application.DTO;
using Infrastructure.Repositories.Customers;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
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
            private readonly DataContext _context;
            private readonly ICustomerRepo _customerRepo;

            public CommandHandler(IActivityService activityService,
                    DataContext context, 
                    ICustomerRepo customerRepo )
            {
                _activityService = activityService;
                _context = context;
                _customerRepo = customerRepo;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {

                    var contacts = await  _customerRepo.GetContactByGroup(request.SMSActivity.Group);
                    ContactSerialize serialize = new ContactSerialize(contacts.ToList(),request.SMSActivity.MobileNos);
                    List<string> mobileNos = serialize.ExtractSMS();
                    string description = request.SMSActivity.Group +
                                        (!string.IsNullOrEmpty(request.SMSActivity.MobileNos) ? request.SMSActivity.MobileNos : "");
                    
                    var activity = await _activityService.CreateBulkSMS(request.SMSActivity.Title,description,mobileNos,
                                              request.SMSActivity.Message,
                                              request.SMSActivity.DateToSend);

                    var campaign = await _context.Campaigns
                                    .Where(c => c.Id == request.SMSActivity.CampaignId)
                                    .FirstOrDefaultAsync();
                    
                    
                    if (campaign == null) 
                        throw new Exception("campaign doesn't exist");
                    
                    campaign.AddActivity(activity);
                    
                    var result = await _context.SaveChangesAsync() > 0;
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
