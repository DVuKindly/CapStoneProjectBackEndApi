using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Auth.Request
{
    public class ResetPasswordRequest
    {

        [Required]
        public string Token { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }
    }
}
