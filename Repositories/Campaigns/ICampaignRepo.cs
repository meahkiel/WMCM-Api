using Core.Campaigns;
using Repositories.Base;
using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Campaigns
{
    public interface ICampaignRepo : IRepositoryBase<Campaign>
    {
        Task<IEnumerable<Campaign>> GetActiveCampaigns();
        Task<Campaign> GetSingleCampaign(Guid id);

        

        
    }
}
