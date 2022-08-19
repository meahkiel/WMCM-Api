using Infrastructure.Core;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.External.SMS
{
    public interface ISMSService
    {
        Task<MessageResource> SendSMS(SMSFormValue value);
    }
}
