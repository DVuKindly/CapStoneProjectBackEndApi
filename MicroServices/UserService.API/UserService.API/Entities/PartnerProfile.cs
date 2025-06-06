using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    public class PartnerProfile
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string AccountId { get; set; } = null!;

        [ForeignKey(nameof(AccountId))]
        public UserProfile? UserProfile { get; set; }

        [MaxLength(255)]
        public string? OrganizationName { get; set; }

        [MaxLength(100)]
        public string? PartnerType { get; set; } // e.g., Strategic, Media, Academic...

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(500)]
        public string? ContractUrl { get; set; }

        public bool IsActivated { get; set; }
        public DateTime? ActivatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedByAdminId { get; set; }

        // 🔽 Bổ sung thêm các trường sau:
        [MaxLength(255)]
        public string? RepresentativeName { get; set; }  // Người đại diện

        [MaxLength(100)]
        public string? RepresentativePhone { get; set; }

        [MaxLength(100)]
        public string? RepresentativeEmail { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; } // Mô tả tổng quan đối tác

        [MaxLength(255)]
        public string? WebsiteUrl { get; set; }

        [MaxLength(255)]
        public string? Industry { get; set; } // Lĩnh vực hoạt động

        public DateTime? JoinedAt { get; set; } // Thời điểm bắt đầu hợp tác
    }
}
