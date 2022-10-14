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
    public class MarketingTask : BaseEntity
    {
        public string Title { get; set; } = "";
        public string UserName { get; set; } = "";
        public bool Close { get; set; } = false;

       
        public ICollection<SubTask> SubTasks {get;set; } = new List<SubTask>();

        public void AddSubTask(string task,string assignedTo,string assignedBy)
        {       
            SubTasks.Add(new SubTask {Id = Guid.NewGuid(), Task = task, AssignedTo = assignedTo,AssignedBy = assignedBy, Status = StatusEnum.Todo.ToString() });
        }

        

       


    }
}
