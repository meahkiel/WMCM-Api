using System.Collections.Generic;

namespace API.DTOs
{
    public class UserDto
    {

        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }

        public string JobTitle { get; set; }

        public string Department { get; set; }
        public string Image { get; set; }

        public List<string> Roles { get; set; }
    }
}
