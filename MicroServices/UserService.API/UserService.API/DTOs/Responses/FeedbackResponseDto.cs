using UserService.API.DTOs.Requests;

namespace UserService.API.DTOs.Responses
{
    public class FeedbackResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public int OverallRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<FeedbackDetailDto> Details { get; set; } = new();
    }
}
