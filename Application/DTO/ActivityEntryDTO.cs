using Core.Campaigns;
using Microsoft.AspNetCore.Http;
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

        public string Id { get; set; }
        public Guid CampaignId { get; set; }

        public string Title { get; set; }

        private string _description;
        public string Description { 
            get { return TruncateAtWord(_description, 180); } set { _description = value; } }
        public DateTime? DateToSend { get; set; }
        public bool IsRecurrent { get; set; } = false;
        public int Days { get; set; } = 0;

        public string Type { get; set; }
        public string ToGroup { get; set; }
        public string To { get; set; }
        public string ToCc { get; set; }
        public string ToBcc { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public string CoverImage { get; set; }
        public IFormFile CoverImageFile { get; set; }

        public string Unicode { get; set; }

        public string Status { get; set; }

        public string TruncateAtWord(string input, int length)
        {
            if (input == null || input.Length < length)
                return input;

            int iNextSpace = input.LastIndexOf(" ", length);

            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

    }
}
