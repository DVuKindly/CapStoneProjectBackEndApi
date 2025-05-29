using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
