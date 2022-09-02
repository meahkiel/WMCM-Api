using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class CampaignDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public IEnumerable<ActivityCampaignDTO> Activities { get; set; }

    }

    
    public record ActivityCampaignDTO
    {
        public string Title { get; init; }
        public string Type { get; init; }
        public DateTime DateCreated { get; init; }
        public string Status { get; init; }



    }
}
