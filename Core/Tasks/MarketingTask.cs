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

        public void AddSubTask(string task,string assignedTo)
        {       
            SubTasks.Add(new SubTask {Id = Guid.NewGuid(), Task = task, AssignedTo = assignedTo, Status = StatusEnum.Todo.ToString() });
        }

        

        public void AttachSubTaskRange(List<SubTask> subTasks)
        {
            foreach(var subTask in subTasks)
            {
                var existing = SubTasks.Where(s => s.Id == subTask.Id).FirstOrDefault();

                if (existing != null)
                {
                    existing = new SubTask { 
                        Id = existing.Id,
                        Task = subTask.Task, 
                        AssignedTo = subTask.AssignedTo, 
                        Status = subTask.Status };
                }
                else
                {
                    AddSubTask(subTask.Task,subTask.AssignedTo);
                }
            }
        }


    }
}
