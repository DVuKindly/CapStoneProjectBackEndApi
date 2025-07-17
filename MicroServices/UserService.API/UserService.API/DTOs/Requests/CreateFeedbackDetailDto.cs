using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class CreateFeedbackDetailDto
    {
        [Required]
        public string ServiceType { get; set; } = null!; // "Room", "Coach", v.v.

        public Guid? ServiceTargetId { get; set; } // RoomInstanceId...

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
