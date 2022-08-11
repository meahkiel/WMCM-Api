using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class SmsDTO
    {
        public Guid CampaignId { get; set; }

        public string Group { get; set; }
        public string MobileNos { get; set; }
        public string Message { get; set; }
        public string Unicode { get; set; }

        public DateTime? DateToSend { get; set; }


    }
}
