namespace UserService.API.DTOs.Responses
{
    public class UserProfileStatusResponse
    {
        public Guid AccountId { get; set; }
        public bool IsCompleted { get; set; }
        public string? OnboardingStatus { get; set; }
    }

}
