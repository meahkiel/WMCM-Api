using Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Campaigns

{
    public class Activity : BaseEntity
    {

        public static Activity CreateSMSActivity(string to, string body,DateTime dispatchDate,DateTime? sendDate = null,string campaignTitle = "")
        {
            var details = new List<ActivityDetail>();
            var activity = new Activity();
            activity.DispatchDate = dispatchDate;
            activity.DateSchedule = sendDate.HasValue ? sendDate.Value : DateTime.Now;
            activity.Title = $"{campaignTitle}";
            activity.Description = $"{body}";
            activity.Type = "sms";
            activity.AddDetail(new ActivityDetail
            {
                Key = "message",
                Value = body
            });
            activity.AddDetail(new ActivityDetail
            {
                Key = "to",
                Value = to
            });

            return activity; 
            
        }
       
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime? DateSchedule { get; set; } = null;

        public DateTime DispatchDate { get; set; }
        public Campaign Campaign { get; set; }

        private List<ActivityDetail> _details = new List<ActivityDetail>();

        public IEnumerable<ActivityDetail> Details => new List<ActivityDetail>(_details);

        public void AddDetail(ActivityDetail detail)
        {
            _details.Add(detail);
        }

    }
}
