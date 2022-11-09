using Application.Interface;
using Core.Campaigns;
using Infrastructure.External.Email;
using Infrastructure.External.SMS;
using Infrastructure.External.Web;
using Microsoft.EntityFrameworkCore;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services
{

    public class ActivityService : IActivityService
    {
       
        private readonly ISMSService _smsService;
        private readonly UnitWrapper _wrapper;
        private readonly ISendSMTPClient _sendSMTPClient;
        private readonly IWebPostService _webPostService;

        public ActivityService(ISMSService smsService,
            UnitWrapper wrapper,
            ISendSMTPClient sendSMTPClient) {   
            _smsService = smsService;
            _wrapper = wrapper;
            _sendSMTPClient = sendSMTPClient;
        }

        public async Task<bool> CreateBulkSMS(List<string> mobileNos, string message) {
            
            List<MessageResource> messageResources = new List<MessageResource>();
            var smsActivity = await _wrapper.Channels.FindByTypeAsync("sms");

            _smsService.ApiKey = smsActivity.ApiKey;
            _smsService.ApiSecret = smsActivity.ApiSecretKey;
            
            //start sending the message
            foreach (var mobileNo in mobileNos) {
                MessageResource messageBroker = await _smsService
                            .SendSMS(new Core.SMSFormValue(mobileNo, "", message));
            }

            return true;

        }

        public async Task<bool> CreateEmail(List<string> emailAdds, string subject, string body) {

            var smsActivity = await _wrapper.Channels.FindByTypeAsync("email");

            _sendSMTPClient.Email = smsActivity.Email;
            _sendSMTPClient.Port = smsActivity.Port;
            _sendSMTPClient.Host = smsActivity.Host;
            _sendSMTPClient.UserName = smsActivity.UserName;
            _sendSMTPClient.Password = smsActivity.Password;


            foreach (var email in emailAdds)
            {
                var emailParam = new EmailParameter {
                    Subject = subject,
                    Body = body,
                    To = email
                };

                await _sendSMTPClient
                            .SendEmailAsync(emailParam);
            }

            return true;
        }

       
    }
}
