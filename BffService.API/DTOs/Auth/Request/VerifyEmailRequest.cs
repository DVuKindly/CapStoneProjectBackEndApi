using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Request
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
