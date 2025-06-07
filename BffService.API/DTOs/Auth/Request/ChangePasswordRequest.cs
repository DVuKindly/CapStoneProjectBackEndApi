using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Request
{
    public class ChangePasswordRequest
    {
   [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
