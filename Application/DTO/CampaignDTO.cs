using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CampaignDTO
    {

        public static CampaignDTO CreateDTO(Guid id,string title,string description,DateTime from,DateTime to)
        {
            return new CampaignDTO
            {
                Id = id,
                Title = title,
                Description = description,
                DateFrom = from,
                DateTo = to,
            };
        }
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }


        public ICollection<ActivityEntryDTO> Activities { get; set; } = new List<ActivityEntryDTO>();

    }
   
}
