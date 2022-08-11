using Core.Base;
using System;
using System.Collections.Generic;

namespace Core.Campaigns
{
    public class Campaign : BaseEntity
    {

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        private List<Activity>  _activities = new List<Activity>();

        public IEnumerable<Activity> Activities => new List<Activity>(_activities);

        public void AddActivity(Activity activity)
        {
            _activities.Add(activity);  

        }




    }
}
