using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    public class Feedback
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AccountId { get; set; } // Người đánh giá
        public Guid PackageId { get; set; } // ❗️Thay MembershipId bằng PackageId

        [Range(1, 5)]
        public int OverallRating { get; set; }

        [MaxLength(2000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public UserProfile? User { get; set; }
        public Membership? Membership { get; set; }

        public ICollection<FeedbackDetail> Details { get; set; } = new List<FeedbackDetail>();
    }
}
