using BffService.API.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Request
{
    public class AdminAccountRegisterAdapter
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        // ✅ Không required vì có thể bỏ qua nếu SkipPasswordCreation = true
        public string? Password { get; set; }

        [Required]
        public string RoleKey { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        public bool SkipPasswordCreation { get; set; } = false; // 🔥 Mặc định là false
        public bool SkipEmailVerification { get; set; } = true;

        /// <summary>
        /// Thông tin động cho coach/partner/staff/supplier
        /// </summary>
        public IProfileInfoRequest? ProfileInfo { get; set; }
    }
}
