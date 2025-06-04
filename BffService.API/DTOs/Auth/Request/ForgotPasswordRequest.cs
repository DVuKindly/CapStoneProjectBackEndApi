using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Auth.Request
{
    public class ForgotPasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
