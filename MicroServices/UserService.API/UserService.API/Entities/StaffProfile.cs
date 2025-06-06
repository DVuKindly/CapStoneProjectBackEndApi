using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    public class StaffProfile
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string AccountId { get; set; } = null!;

        [ForeignKey(nameof(AccountId))]
        public UserProfile? UserProfile { get; set; }

        [MaxLength(100)]
        public string? StaffGroup { get; set; } 

        
        [MaxLength(100)]
        public string? Department { get; set; } 

        [MaxLength(50)]
        public string? Level { get; set; } 

        [MaxLength(100)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? ManagerId { get; set; }

        [MaxLength(255)]
        public string? Note { get; set; }

        public DateTime? JoinedDate { get; set; } // Ngày bắt đầu làm việc

        public bool IsActive { get; set; } = true; // Trạng thái đang làm việc
    }
}
