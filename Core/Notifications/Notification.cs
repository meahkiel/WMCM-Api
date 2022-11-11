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
        public static Notification Create(string module,string description,string userId)
        {
            return new Notification
            {
                Module = module,
                Description = description,
                UserId = userId,
                HasRead = false
            };
        }
        public string Module { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public bool HasRead { get; set; }


    }
}
