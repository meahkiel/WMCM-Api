using SMSService.Application.Common.DTO;

namespace SMSService.Application.Interfaces
{
    public interface ISMSService
    {
        Task<ServiceResult> Execute(string message, string from, string to);
    }
}
