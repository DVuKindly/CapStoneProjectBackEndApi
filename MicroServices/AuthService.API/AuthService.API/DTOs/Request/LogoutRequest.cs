using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class LogoutRequest
    {
        [Required]
        public string UserId { get; set; }
    }
}
