using Application.Interface;
using Application.SeedWorks;
using Core.Campaigns;
using Core.Channels;
using Infrastructure.External.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Campaigns
{
    public class SMTPActivityService : IActivityServiceAccessor
    {

        private readonly ISendSMTPClient _sendSMTPClient;
        private readonly ChannelSetting _setting;
       

        public SMTPActivityService(ISendSMTPClient sendSMTPClient, ChannelSetting setting)
        {
            _sendSMTPClient = sendSMTPClient;
            _setting = setting;
        }

        public async Task<ServiceMessageBroker> Execute(Activity activity)
        {
            if (activity.DateSchedule == null || activity.DateSchedule <= DateTime.Now)
            {   
                List<string> emailAddress = new List<string>(activity.To.Split(",", StringSplitOptions.TrimEntries));
                await Execution(emailAddress, activity.Subject, activity.Body);
                return new ServiceMessageBroker(DateTime.Today, "Success", true);
            }

            return  new ServiceMessageBroker(DateTime.Today, "", false);
        }

      

        public async Task Execution(List<string> emailAdds, string subject, string body)
        {

            _sendSMTPClient.Email = _setting.Email;
            _sendSMTPClient.Port = _setting.Port;
            _sendSMTPClient.Host = _setting.Host;
            _sendSMTPClient.UserName = _setting.UserName;
            _sendSMTPClient.Password = _setting.Password;

            foreach (var email in emailAdds)
            {
                var emailParam = new EmailParameter
                {
                    Subject = subject,
                    Body = body,
                    To = email
                };

                await _sendSMTPClient
                            .SendEmailAsync(emailParam);
            }
        }

       
       
       
    }
}
