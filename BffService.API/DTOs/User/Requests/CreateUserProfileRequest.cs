using System;

namespace BffService.API.DTOs.Requests
{
    public class CreateUserProfileRequest
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string RoleType { get; set; } = "User";
        public Guid LocationId { get; set; }
        public string? OnboardingStatus { get; set; }
        public string? Note { get; set; }
        public Guid? CreatedByAdminId { get; set; }
    }
}
