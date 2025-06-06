using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class CreateStaffProfileRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [MaxLength(100)]
        public string? Phone { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        [MaxLength(255)]
        public string? Note { get; set; }

        [MaxLength(50)]
        public string? Level { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        // ✅ Đã sửa: string? => Guid?
        public Guid? ManagerId { get; set; }
    }
}
