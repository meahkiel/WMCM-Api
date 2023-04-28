using Core.Campaigns;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Campaigns
{
    public interface ICampaignRepo : IRepositoryBase<Campaign>
    {
        Task<IEnumerable<Campaign>> GetActiveCampaigns();

        Task<Activity?> GetActivityByType(string type);
        
        Task<Campaign> GetSingleCampaign(Guid id);

        Task<Activity> GetByActivity(string activityId);

        void UpdateActivity(Activity activity);

        void DeleteActivity(Activity activity);

        void AddActivity(Activity activity);

        

        
    }
}
