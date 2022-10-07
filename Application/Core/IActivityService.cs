using Core.Campaigns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Core
{
    public interface IActivityService
    {
        Task<bool> CreateBulkSMS(List<string> mobileNos, string message);

        Task<bool> CreateEmail(List<string> emailAdds, string subject, string body);
    }
}
