using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSService.Domain
{
    public class SMSActivity
    {


        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime? DateSchedule { get; set; } = null;
        public bool IsRecurrent { get; set; } = false;
        public int Days { get; set; } = 0;

        public DateTime? DispatchDate { get; set; }
        public string Receipient { get; set; } = null!;
        public string From { get; set; } = null!;

        public string Body { get; set; } = null!;


        public string Status { get; set; } = null!;
    }
}
