using Core.Campaigns;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IActivityService
    {
        Task<bool> CreateBulkSMS(List<string> mobileNos, string message);

        Task<bool> CreateEmail(List<string> emailAdds, string subject, string body);

       

    }
}
