using Core.Campaigns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Core
{
    public interface IActivityService
    {
        Task<Activity> CreateBulkSMS(string title, string description, List<string> mobileNos, string message, DateTime? sendDate);

    }
}
