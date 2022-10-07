using Application.Core;
using Core.Campaigns;
using Infrastructure.External.Email;
using Infrastructure.External.SMS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services
{

    public class ActivityService : IActivityService
    {
       
        private readonly ISMSService _smsService;
        private readonly ISendSMTPClient _sendSMTPClient;

        public ActivityService(ISMSService smsService,
            ISendSMTPClient sendSMTPClient) {   
            _smsService = smsService;
            _sendSMTPClient = sendSMTPClient;
        }

        public async Task<bool> CreateBulkSMS(List<string> mobileNos, string message) {
            
            List<MessageResource> messageResources = new List<MessageResource>();

            //start sending the message
            foreach (var mobileNo in mobileNos) {
                MessageResource messageBroker = await _smsService
                            .SendSMS(new Core.SMSFormValue(mobileNo, "", message));
            }

            return true;

        }

        public async Task<bool> CreateEmail(List<string> emailAdds, string subject, string body) {
            
            foreach (var email in emailAdds)
            {
                var emailParam = new EmailParameter {
                    Subject = subject,
                    Body = body,
                    To = email
                };

                await _sendSMTPClient.SendEmailAsync(emailParam);
            }

            return true;
        }
    }
}
