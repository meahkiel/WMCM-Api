

using System;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class MarketingTaskDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string UserName { get; set; } = "";
        public bool Close { get; set; } = false;

        public int TotalSubTaskCount { get; set; }

        public List<MarketingSubTaskDTO> SubTasks { get; set; }

    }

    public class MarketingSubTaskDTO
    {
        public string Task { get; set; } = "";

        public string AssignedTo { get; set; } = null;

        public string Status { get; set; } = "";
    }
}
