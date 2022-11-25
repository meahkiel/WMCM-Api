namespace SMSService.Application.Common.DTO
{
    public class SMSActivityRequest
    {
        public string Id { get; set; } = null!;
        public string Receipient { get; set; } = null!;
        public string From { get; set; } = null!;

        public string Body { get; set; } = null!;

        public DateTime DateSchedule { get; set; } = DateTime.Today;
        


    }
}
