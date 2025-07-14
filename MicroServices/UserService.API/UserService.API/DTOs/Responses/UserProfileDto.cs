using System;

namespace UserService.API.DTOs.Responses
{
    public class UserProfileDto
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; } = "";
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? AvatarUrl { get; set; }
        public string? SocialLinks { get; set; }
        public string? Address { get; set; }
      

    
        public List<string>? Interests { get; set; }
        public List<string>? PersonalityTraits { get; set; }





        public string? Introduction { get; set; }
        public string? CvUrl { get; set; }

        public Guid? LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? OnboardingStatus { get; set; }
        public string? Note { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


}
