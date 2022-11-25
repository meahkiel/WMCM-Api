namespace SMSService.Application.Common.Settings
{
    public class SMTPSettings
    {
        public string Email { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
        public string From { get; set; } = null!;
    }
}
