using SMSService.Application.Common;

namespace SMSService.Application.Interfaces
{
    public interface ISendSMTPClient
    {
        Task SendMailAsync(EmailParameter parameter);
    }
}
