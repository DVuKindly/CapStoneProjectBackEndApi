using System;

namespace UserService.API.DTOs.Requests
{
    public class UpdateUserProfileRequest
    {
        public Guid AccountId { get; set; } 
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; } 
        public string? Location { get; set; }
        public string? AvatarUrl { get; set; }
        public string? SocialLinks { get; set; }

        public bool? IsCompleted { get; set; }
        public bool? IsVerifiedByAdmin { get; set; }

        public string? OnboardingStatus { get; set; }

        // Nếu dùng cập nhật Role (có thể không cho user tự chỉnh)
        public string? RoleType { get; set; }
    }
}
