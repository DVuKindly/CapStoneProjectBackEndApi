using System;

namespace UserService.API.DTOs.Requests
{
    public class UserProfilePayload
    {
        public Guid AccountId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? RoleType { get; set; }
        public Guid LocationId { get; set; }
        public Guid? CreatedByAdminId { get; set; }
        public string? OnboardingStatus { get; set; }
        public string? Note { get; set; }

        // Common
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; }
        public string? Address { get; set; }

        // Staff
        public string? Department { get; set; }
        public string? Level { get; set; }
        public Guid? ManagerId { get; set; }

        // Coach
        public string? CoachType { get; set; }
        public string? Specialty { get; set; }
        public string? Module { get; set; }
        public string? Certifications { get; set; }
        public string? LinkedInUrl { get; set; }




        public List<Guid>? InterestIds { get; set; }
        public List<Guid>? PersonalityTraitIds { get; set; }



        // Partner
        public string? OrganizationName { get; set; }
        public string? PartnerType { get; set; }
        public string? RepresentativeName { get; set; }
        public string? RepresentativePhone { get; set; }
        public string? RepresentativeEmail { get; set; }
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Industry { get; set; }

        // Supplier
        public string? CompanyName { get; set; }
        public string? TaxCode { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
    }
}
