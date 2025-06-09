using AuthService.API.DTOs.STAFF;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthService.API.DTOs.AdminCreate
{
    public class RegisterStaffRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;


        [Required]
        public Guid LocationId { get; set; }

        public bool SkipPasswordCreation { get; set; } = true;
      

        [JsonIgnore] // Gán trong controller
        public string RoleKey { get; set; } = string.Empty;

        [Required]
        public StaffProfileInfoRequest ProfileInfo { get; set; } = null!;
    }
}
