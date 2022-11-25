namespace SMSService.Application.Common
{
    public class EmailParameter
    {
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
