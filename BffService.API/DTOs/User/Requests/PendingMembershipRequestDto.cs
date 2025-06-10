namespace BffService.API.DTOs.Requests
{
    public class PendingMembershipRequestDto
    {
        public Guid RequestId { get; set; }
        public string? FullName { get; set; }
        
        public string? RequestedPackageName { get; set; }
        public string? MessageToStaff { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LocationName { get; set; }

        // Từ UserProfile
        public string? Interests { get; set; }
        public string? PersonalityTraits { get; set; }
        public string? Introduction { get; set; }
        public string? CvUrl { get; set; }

        public ExtendedProfileDto? ExtendedProfile { get; set; }
    }

    public class ExtendedProfileDto
    {
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? AvatarUrl { get; set; }
        public string? SocialLinks { get; set; }
        public string? Address { get; set; }
        public string? RoleType { get; set; }
    }

}
