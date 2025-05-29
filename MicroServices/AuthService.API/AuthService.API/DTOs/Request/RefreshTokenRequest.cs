using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
