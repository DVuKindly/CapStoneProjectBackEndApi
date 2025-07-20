using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.API.Entities
{
    public class FeedbackDetail
    {
        [Key]
        public Guid Id { get; set; }

        public Guid FeedbackId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ServiceType { get; set; } = null!; // e.g., Room, Coach, Event

        public Guid? ServiceTargetId { get; set; } // RoomInstanceId, CoachId...

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public Feedback? Feedback { get; set; }
    }
}
