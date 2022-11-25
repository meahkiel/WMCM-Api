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
        Task<Campaign> GetSingleCampaign(Guid id);

        Task<Campaign> GetByActivity(string activityId);
        void DeleteActivity(Activity activity);

        void AddActivity(Activity activity);

        

        
    }
}
