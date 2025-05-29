using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }
    }
}
