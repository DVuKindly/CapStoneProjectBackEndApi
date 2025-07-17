using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class CreateFeedbackDto
    {
        public Guid PackageId { get; set; }
        public int OverallRating { get; set; } // Gói
        public string? Comment { get; set; }
        public List<FeedbackDetailInputDto>? Details { get; set; }
    }

    public class FeedbackDetailInputDto
    {
        public string ServiceType { get; set; } = default!;
        public Guid ServiceTargetId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }


}
