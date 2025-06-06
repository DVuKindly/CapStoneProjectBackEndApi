namespace UserService.API.DTOs.Responses
{
    public class PendingMembershipRequestResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid PackageId { get; set; }
        public string RequestedPackageName { get; set; } = null!;
        public string? Location { get; set; }
        public string Status { get; set; } = null!;
        public string? StaffNote { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // User Info (from UserProfile)
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? RoleType { get; set; }
        public string? OnboardingStatus { get; set; }

        // Optional - Info from MembershipService (if you join or call API)
        public string? PackageDescription { get; set; }
        public decimal? Price { get; set; }
        public int? DurationDays { get; set; }
        public List<string>? FeatureLabels { get; set; }
    }

}
