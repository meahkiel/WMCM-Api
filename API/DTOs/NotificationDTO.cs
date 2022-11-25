using System;

namespace API.DTOs
{
    public class NotificationDTO
    {
        public string Id { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasRead { get; set; }
    }
}
