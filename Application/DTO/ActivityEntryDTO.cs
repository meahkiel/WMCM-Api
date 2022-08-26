using Core.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{

    public record ActivityInitial (string[] Groups,List<Template> Templates);
    public class ActivityEntryDTO
    {
        public Guid CampaignId { get; set; }
        public string Title { get; set; }
        public DateTime? DateToSend { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public string MobileNos { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Unicode { get; set; }

        public string Status { get; set; }

    }
}
