using Application.DTO;
using Application.Interface;
using Application.SeedWorks;
using Core.Campaigns;
using Core.Channels;
using Infrastructure.External.SMS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services.Campaigns
{
    public class SMSActivityService : IActivityServiceAccessor
    {

        private readonly ISMSService _smsService;
        private readonly ChannelSetting _setting;

        public SMSActivityService(ISMSService smsService, ChannelSetting setting)
        {
            _smsService = smsService;
            _setting = setting;
        }

        

        public async Task<ServiceMessageBroker> Execute(Activity activity)
        {
            
            if (activity.DateSchedule == null || activity.DateSchedule <= DateTime.Now)
            {
                List<string> mobileNos = new List<string>(activity.To.Split(",", StringSplitOptions.TrimEntries));
                await SendSMS(activity.Body, mobileNos, _setting);
                return new ServiceMessageBroker(DateTime.Today, "Success", true);
            }

            return new ServiceMessageBroker(DateTime.Today, "", false);
        }

        private async Task SendSMS(string body, List<string> mobileNos, ChannelSetting smsActivity)
        {
            _smsService.ApiKey = smsActivity.ApiKey;
            _smsService.ApiSecret = smsActivity.ApiSecretKey;
            _smsService.FromPhone = smsActivity.PhoneNo;
            //start sending the message
            foreach (var mobileNo in mobileNos) 
            {
                MessageResource messageBroker = await _smsService
                            .SendSMS(new Core.SMSFormValue(mobileNo, "",body));
            }
        }
        
    }
}
