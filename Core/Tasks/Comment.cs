using Core.Base;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tasks
{
    [Auditable]
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public string UserName { get; set; }

        public SubTask SubTask { get; set; }
    }
}
