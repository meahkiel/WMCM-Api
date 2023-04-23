using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SeedWorks
{
    public class ServiceMessageBroker
    {
        public ServiceMessageBroker()
        {

        }

        public ServiceMessageBroker(DateTime dispatchDate,string response, bool isSuccess)
        {
            DispatchDate = dispatchDate;
            Response = response;
            IsSuccess = isSuccess;
        }
        public DateTime DispatchDate { get; init; }
        public string Response { get; init; }
        public bool IsSuccess { get; set; }
    }
}
