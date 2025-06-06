using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Auth.RequestProfileUser
{
    public class AdminRegisterRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string RoleKey { get; set; } = string.Empty;

        public bool SkipEmailVerification { get; set; } = false;

        public ProfileInfoRequest? ProfileInfo { get; set; }
    }

}
