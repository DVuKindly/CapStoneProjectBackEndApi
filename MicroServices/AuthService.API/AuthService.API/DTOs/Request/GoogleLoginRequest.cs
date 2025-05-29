using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class GoogleLoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ProviderId { get; set; } // Google UID
    }
}
