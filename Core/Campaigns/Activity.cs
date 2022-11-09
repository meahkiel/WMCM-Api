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
            DateTime dispatchDate,
            DateTime? sendDate = null)
        {
            
            var activity = new Activity();

            activity.DispatchDate = dispatchDate;
            activity.DateSchedule = sendDate.HasValue ? sendDate.Value : DateTime.Now;
            activity.Title = $"{title}";
            activity.Description = $"Sent Message {to}";
            activity.Type = "sms";
            activity.To = to;
            activity.Body = body;

            return activity; 
        }

        public static Activity CreateEmailActivity(
            string title,
            string description,
            string subject, 
            string body, 
            DateTime dispatchDate,string to, 
            DateTime? sendDate = null)
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
            activity.To = to;

            return activity;
        }

        public static Activity CreateWebPost(
            string title, 
            string body,
            string to,
            string coverImage,
            DateTime dispatchDate,  
            DateTime? sendDate = null)
        {

            var activity = new Activity();

            activity.DispatchDate = dispatchDate;
            activity.DateSchedule = sendDate.HasValue ? sendDate.Value : DateTime.Now;
            activity.Title = $"{title}";
            activity.Description = $"Campaign Web Post ";
            activity.Type = "web";
            activity.CoverImage = coverImage;
            activity.Body = body;
            activity.To = to;

            return activity;
        }



        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime? DateSchedule { get; set; } = null;

        public bool IsRecurrent { get; set; } = false;
        public int Days { get; set; } = 0;

        public DateTime DispatchDate { get; set; }
        
        public virtual Campaign Campaign { get; set; }

        public string To { get; set; }

        public string EmailCc { get; set; }

        public string From { get; set; }

        public string Subject { get; set; }

        public string CoverImage { get; set; } = null;

        public string Body { get; set; }
       

        public string Status { get; set; }
      
    }
}
