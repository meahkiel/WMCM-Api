﻿

using Core.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.DTOs
{

    public record  MarketingSubTaskDTO(string Id,string Task,string AssignedTo,string Status);

    public class MarketingTaskDTO {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string UserName { get; set; } = "";
        public bool Close { get; set; } = false;
        public int TotalSubTaskCount => SubTasks.Where(s => s.Status != StatusEnum.Done.ToString()).Count();
        public List<MarketingSubTaskDTO> SubTasks { get; set; }
        public List<string> Users => SubTasks.Select(s => s.AssignedTo).Distinct().ToList();
    }

   
}
