using Core.Base;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tasks
{

    public enum StatusEnum
    {
        Todo,
        Doing,
        Done
    }

    [Auditable]
    public class SubTask : BaseEntity
    {
        public string Task { get; set; } = "";

        public string AssignedTo { get; set; } = null;

        public string Status { get; set; } = "";

        public void UpdateStatus(StatusEnum value)
        {
            Status = value.ToString();
        }

        public MarketingTask MarketingTask { get; set; }
        
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();


    }
}
