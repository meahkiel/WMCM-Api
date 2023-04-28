using System;

namespace Core.Channels
{
    public class ChannelSetting
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Type { get; set; }
        public string BaseUrl { get; set; } = "";
        public string ApiKey { get; set; } = ""; 
        public string ApiSecretKey { get; set; } = "";

        public string Header { get; set; }

        public string Host { get; set; } = "";

        public int Port { get; set; } = 0;

        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";

        public string Email { get; set; } = "";
        public string PhoneNo { get; set; } = "";

        public bool IsDisabled { get; set; } = false;






    }
}
