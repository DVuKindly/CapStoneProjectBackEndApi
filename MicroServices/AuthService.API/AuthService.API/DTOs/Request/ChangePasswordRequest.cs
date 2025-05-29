using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
