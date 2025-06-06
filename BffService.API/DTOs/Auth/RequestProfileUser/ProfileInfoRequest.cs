namespace BffService.API.DTOs.Auth.RequestProfileUser
{
    public class ProfileInfoRequest
    {
        // Common
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; }
        public string? Location { get; set; }
        public string? OnboardingStatus { get; set; }
        public string? Note { get; set; }

        // Coach
        public string? CoachType { get; set; }
        public string? Module { get; set; }
        public string? Specialty { get; set; }

        // Staff
        public string? Level { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public string? ManagerId { get; set; }

        // Partner
        public string? OrganizationName { get; set; }
        public string? PartnerType { get; set; }
        public string? ContractUrl { get; set; }
        public string? RepresentativeName { get; set; }
        public string? RepresentativePhone { get; set; }
        public string? RepresentativeEmail { get; set; }
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Industry { get; set; }
    }
}
