using Core.Base;
using Core.Enum;
using Core.Extensions;
using System.Collections.Generic;

namespace Core.Tasks
{



    [Auditable]
    public class SubTask : BaseEntity
    {
        public string Task { get; set; } = "";

        public string AssignedTo { get; set; } = null;

        public string AssignedBy { get; set; }

        public string Status { get; set; } = "";

        public void UpdateStatus(TaskStatusEnum value)
        {
            Status = value.ToString();
        }

        public MarketingTask MarketingTask { get; set; }
        
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();


    }
}
