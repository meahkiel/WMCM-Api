using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.Services.SMS
{
    public interface ISMSService
    {

        Task<MessageResource> SendSMS(string from, string body);
    }
}
