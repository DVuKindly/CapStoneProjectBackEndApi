using System;

namespace UserService.API.DTOs.Responses
{
    public class UserProfileDto
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string RoleType { get; set; } = null!;
        public Guid LocationId { get; set; }
        public string? OnboardingStatus { get; set; }
        public string? Note { get; set; }

        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
