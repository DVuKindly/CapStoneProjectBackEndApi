using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Auth.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
