using Infrastructure.Services.Credential;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services.SMS
{
    public class TwilioSMS : ISMSService
    {
        private readonly TwilioSettings _settings;

        public TwilioSMS(IOptions<TwilioSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<MessageResource> SendSMS(string to, string body)
        {
            TwilioClient.Init(_settings.AccountSID, _settings.AuthToken);
            var result = await MessageResource.CreateAsync(
                body: body,
                from: new Twilio.Types.PhoneNumber(_settings.PhoneNo),
                to: new Twilio.Types.PhoneNumber(to));

            return result;

        }
    }
}
