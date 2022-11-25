using Microsoft.Extensions.Options;
using SMSService.Application.Common.DTO;
using SMSService.Application.Common.Settings;
using SMSService.Application.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SMSService.Infrastructure.Services
{
    public class SMSService : ISMSService
    {

        private readonly string _apiKey;
        private readonly string _apiSecret;

        public SMSService(IOptions<SMSSettings> options)
        {
            _apiKey = options.Value.AccountSID;
            _apiSecret = options.Value.AuthToken;
        }


        public async Task<ServiceResult> Execute(string message, string from, string to)
        {
            try
            {

                TwilioClient.Init(_apiKey, _apiSecret);

                var result = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(from),
                    to: new Twilio.Types.PhoneNumber(to));

                return new ServiceResult("", DateTime.Now, "Succesful");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
