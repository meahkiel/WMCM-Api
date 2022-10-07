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

        public static Activity CreateSMSActivity(
            string title,
            string to, 
            string body,
            DateTime dispatchDate,DateTime? sendDate = null,string campaignTitle = "")
        {
            
            var activity = new Activity();

            activity.DispatchDate = dispatchDate;
            activity.DateSchedule = sendDate.HasValue ? sendDate.Value : DateTime.Now;
            activity.Title = $"{title}";
            activity.Description = $"Sent Message {to}";
            activity.Type = "sms";
            activity.Status = "Completed";
            activity.To = to;
            activity.Body = body;

            return activity; 
        }

        public static Activity CreateEmailActivity(string title,string description,string subject, string body, DateTime dispatchDate, DateTime? sendDate = null, string campaignTitle = "")
        {

            var activity = new Activity();

            activity.DispatchDate = dispatchDate;
            activity.DateSchedule = sendDate.HasValue ? sendDate.Value : DateTime.Now;
            activity.Title = $"{title}";
            activity.Description = $"{description}";
            activity.Type = "email";
            activity.Status = "Completed";
            activity.Subject = subject;
            activity.Body = body;

            return activity;
        }
       
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime? DateSchedule { get; set; } = null;

        public DateTime DispatchDate { get; set; }
        
        public Campaign Campaign { get; set; }

        public string Status { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
       
        public string To { get; set; }


        private List<ActivityDetail> _details = new List<ActivityDetail>();

        public IEnumerable<ActivityDetail> Details => new List<ActivityDetail>(_details);

        public void AddDetail(ActivityDetail detail)
        {
            _details.Add(detail);
        }

    }
}
