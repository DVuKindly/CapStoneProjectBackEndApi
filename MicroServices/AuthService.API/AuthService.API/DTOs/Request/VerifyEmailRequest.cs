using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
