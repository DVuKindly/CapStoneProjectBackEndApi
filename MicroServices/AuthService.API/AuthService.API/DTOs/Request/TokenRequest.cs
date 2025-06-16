using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.DTOs.Request
{
    public class TokenRequest
    {
        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; } = null!;

        [FromForm(Name = "username")]
        public string? Username { get; set; }

        [FromForm(Name = "password")]
        public string? Password { get; set; }

        [FromForm(Name = "refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
