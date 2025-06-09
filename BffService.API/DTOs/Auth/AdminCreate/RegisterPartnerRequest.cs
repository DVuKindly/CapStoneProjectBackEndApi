using BffService.API.DTOs.PARTNER;
using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.AdminCreate
{
    public class RegisterPartnerRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;


        [Required]
        public Guid LocationId { get; set; }

        public bool SkipPasswordCreation { get; set; } = true;
      

        [Required]
        public PartnerProfileInfoRequest ProfileInfo { get; set; } = null!;
    }
}
