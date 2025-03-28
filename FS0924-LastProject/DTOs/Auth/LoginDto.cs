using System.ComponentModel.DataAnnotations;

namespace LastProject.DTOs.Auth
{
    public class LoginDto
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
