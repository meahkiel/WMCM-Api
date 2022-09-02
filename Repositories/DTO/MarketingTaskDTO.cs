

namespace Repositories.DTOs
{
    public class MarketingTaskDTO
    {
        public string Title { get; set; } = "";
        public string UserName { get; set; } = "";
        public bool Close { get; set; } = false;

        public int TotalSubTaskCount { get; set; }
    }
}
