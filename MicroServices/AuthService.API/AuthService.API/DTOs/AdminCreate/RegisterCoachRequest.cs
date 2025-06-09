using AuthService.API.DTOs.COACH;
using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.AdminCreate
{
    public class RegisterCoachRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;



        [Required]
        public Guid LocationId { get; set; }

        public bool SkipPasswordCreation { get; set; } = true;
        

        [Required]
        public CoachProfileInfoRequest ProfileInfo { get; set; } = null!;
    }
}
