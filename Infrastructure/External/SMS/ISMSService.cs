using Infrastructure.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.External.SMS
{
    public interface ISMSService
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string FromPhone { get; set; }

        Task<MessageResource> SendSMS(SMSFormValue value);
       
    }
}
