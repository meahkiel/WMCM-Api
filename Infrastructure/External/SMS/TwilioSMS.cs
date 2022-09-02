using Infrastructure.Core;
using Infrastructure.External.Credential;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.External.SMS
{
    public class TwilioSMS : ISMSService
    {
        private readonly TwilioSettings _settings;

        public TwilioSMS(IOptions<TwilioSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendBulkSMSAsync(List<string> tos, string from, string message)
        {
            TwilioClient.Init(_settings.AccountSID, _settings.AuthToken);
            foreach (var to in tos)
            {
                var result = await MessageResource.CreateAsync(
                                    body: message,
                                    from: new Twilio.Types.PhoneNumber(_settings.PhoneNo),
                                    to: new Twilio.Types.PhoneNumber(to));
            }

            return true;
        }

        public async Task<MessageResource> SendSMS(SMSFormValue value)
        {
            TwilioClient.Init(_settings.AccountSID, _settings.AuthToken);

            var result = await MessageResource.CreateAsync(
                body: value.Message,
                from: new Twilio.Types.PhoneNumber(_settings.PhoneNo),
                to: new Twilio.Types.PhoneNumber(value.To));

            return result;
        }
    }
}
