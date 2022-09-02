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

        private List<SubTask> _subTasks = new List<SubTask>();
        public IEnumerable<SubTask> SubTasks => new HashSet<SubTask>(_subTasks);

        public void AddSubTask(string task,string assignedTo)
        {
            _subTasks.Add(new SubTask { Task = task, AssignedTo = assignedTo, Status = StatusEnum.Todo.ToString() }); ;
        }


    }
}
