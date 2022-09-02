using Infrastructure.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.External.SMS
{
    public interface ISMSService
    {
        Task<MessageResource> SendSMS(SMSFormValue value);
        Task<bool> SendBulkSMSAsync(List<string> tos, string from, string message);
    }
}
