namespace WCMAPI.DTOs
{
    public class RegisterDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
