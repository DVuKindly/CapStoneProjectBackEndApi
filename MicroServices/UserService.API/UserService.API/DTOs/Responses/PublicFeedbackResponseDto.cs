namespace UserService.API.DTOs.Responses
{
    public class PublicFeedbackResponseDto
    {
        public double AverageRating { get; set; }
        public int TotalCount { get; set; }
        public Dictionary<int, int> StarDistribution { get; set; } = new(); // 1-5 star
        public List<PublicFeedbackItemDto> Feedbacks { get; set; } = new();
    }

    public class PublicFeedbackItemDto
    {
        public string MaskedName { get; set; } = ""; // ví dụ: "Vikin****"
        public string AvatarUrl { get; set; } = "";
        public string Comment { get; set; } = "";
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
