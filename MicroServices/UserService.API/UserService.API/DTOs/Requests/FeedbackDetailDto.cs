namespace UserService.API.DTOs.Requests
{
    public class FeedbackDetailDto
    {
        public string ServiceType { get; set; } = null!;
        public Guid? ServiceTargetId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
