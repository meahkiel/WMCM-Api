

using Core.Enum;
using Core.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.DTOs
{

    public record  MarketingSubTaskDTO(string Id,string Task,string AssignedTo,string? AssignedBy, string? Status,bool MarkDelete = false);

    public class MarketingTaskDTO {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string UserName { get; set; } = "";
        public bool Close { get; set; } = false;


        public int TotalSubTaskCount
        {
            get
            {
                var subTask = SubTasks.Where(s => s.Status != TaskStatusEnum.Done.ToString()).ToList();
                return (subTask != null) ? subTask.Count() : 0;
                
            }
        }

        public List<MarketingSubTaskDTO> SubTasks { get; set; } = new();
        public List<string> Users => SubTasks.Select(s => s.AssignedTo).Distinct().ToList();
    }

   
}
