using Application.DTO;
using Core.Campaigns;
using Infrastructure.Services.SMS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Application.Activities
{
    public class SendSMS
    {
        public class Command : IRequest<Unit>
        {
            public SmsDTO SmsFormValue { get; set; }

        }

       
        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _dataContext;
            private readonly ISMSService _smsService;
            private readonly ILogger<CommandHandler> _logger;   

            public CommandHandler(DataContext context, 
                ISMSService service,
                ILogger<CommandHandler> logger)
            {
                _dataContext = context;
                _smsService = service;
                _logger = logger;

            }
            public async Task<Unit>  Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    //step 1 get contacts that belongs to group
                    var contacts = await _dataContext.Contacts
                        .Where(c => c.GroupTag.Contains(request.SmsFormValue.Group))
                        .ToListAsync();


                    //step 2 dispatch the message
                    foreach(var contact in contacts)
                    {
                       MessageResource messageBroker = await _smsService
                            .SendSMS(contact.PrimaryContact, request.SmsFormValue.Message);
                        _logger.LogInformation(messageBroker.ErrorMessage);
                    }

                    //step 3 fetch the campaign to insert new activity
                    var campaign = await _dataContext.Campaigns
                        .Where(cmp => cmp.Id == request.SmsFormValue.CampaignId)
                        .FirstOrDefaultAsync();

                    //step 4 create an activity
                    var activity = Activity.CreateSMSActivity(
                            request.SmsFormValue.MobileNos,
                            request.SmsFormValue.Message,
                            DateTime.Now,
                            request.SmsFormValue.DateToSend);
                    
                    campaign.AddActivity(activity);

                    _dataContext.Entry(campaign)
                        .State = EntityState.Modified;

                    await _dataContext.SaveChangesAsync();


                } 
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }

                return Unit.Value;
            }
        }
    }
}
