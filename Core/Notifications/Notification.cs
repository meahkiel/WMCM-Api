using Core.Base;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Notifications
{
    [Auditable]
    public class Notification : BaseEntity
    {

        public string Module { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

    }
}
