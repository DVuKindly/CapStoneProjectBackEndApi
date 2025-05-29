using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class ForgotPasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
