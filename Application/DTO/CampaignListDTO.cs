using Core.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{

    public class CampaignListDTO
    {

        public List<CampaignDTO> Campaigns { get; set; } = new List<CampaignDTO>();

        public int TotalSMSActivities { get; set; }
        public int TotalEmailActivities { get; set; }
        public int TotalSocialPost { get; set; }
        public int TotalEcommerce { get; set; }
    }
}
