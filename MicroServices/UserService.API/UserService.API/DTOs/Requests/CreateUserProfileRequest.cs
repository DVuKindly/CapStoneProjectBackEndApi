namespace UserService.API.DTOs.Requests
{
    public class CreateUserProfileRequest
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string RoleType { get; set; } = "User";

        public string? Location { get; set; }
        public string? CoachType { get; set; }
        public string? Module { get; set; }
        public string? Specialty { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; }
        public string? Note { get; set; }
        public string? OnboardingStatus { get; set; }
    }
}
