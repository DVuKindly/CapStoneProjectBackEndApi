using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Request
{
    public class LogoutRequest
    {
        [Required]
        public string UserId { get; set; }
    }
}
