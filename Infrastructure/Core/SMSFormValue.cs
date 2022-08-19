using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    public class SMSFormValue
    {

        public SMSFormValue(string to,string from,string message)
        {
            To = to;
            From = from;
            Message = message;
        }
        public string To { get; private set; }
        public string From { get; set; }
        public string Message { get; private set; }
    }
}
