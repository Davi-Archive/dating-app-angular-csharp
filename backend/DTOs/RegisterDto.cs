using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MinLength(2)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
