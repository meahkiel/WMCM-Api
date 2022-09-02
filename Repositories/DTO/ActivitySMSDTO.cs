using System;

namespace Repositories.DTO
{
    public class ActivitySMSDTO
    {
        public Guid CampaignId { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public string MobileNos { get; set; }
        public string Message { get; set; }
        public string Unicode { get; set; }

        public DateTime? DateToSend { get; set; }




    }
}
