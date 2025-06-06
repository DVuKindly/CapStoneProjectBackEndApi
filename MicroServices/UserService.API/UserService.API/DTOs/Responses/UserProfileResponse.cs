namespace UserService.API.DTOs.Responses
{
    public class UserProfileResponse
    {
        public Guid AccountId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? Location { get; set; }
        public string? RoleType { get; set; }
        public string? OnboardingStatus { get; set; }
        public bool IsCompleted { get; set; }
    }
}
