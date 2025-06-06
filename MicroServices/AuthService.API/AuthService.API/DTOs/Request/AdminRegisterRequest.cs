using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class AdminRegisterRequest
    {
        [Required]
        public string UserName { get; set; } = default!;

        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required, MinLength(6)]
        public string Password { get; set; } = default!;

  
        [Required]
        public string RoleKey { get; set; } = "User";

     
        public bool SkipEmailVerification { get; set; } = false;


        public ProfileInfoRequest? ProfileInfo { get; set; }
    }
}
