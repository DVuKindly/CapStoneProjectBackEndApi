using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class UpdateUserProfileDto
    {
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Full name must be between {2} and {1} characters.")]
        public string? FullName { get; set; }

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must start with 0 and contain exactly 10 digits.")]
        public string? Phone { get; set; }

        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be 'Male', 'Female' or 'Other'.")]
        public string? Gender { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date of birth must be a valid date.")]
        public DateTime? DOB { get; set; }

        public List<Guid>? InterestIds { get; set; }
        public List<Guid>? PersonalityTraitIds { get; set; }

        [Url(ErrorMessage = "Avatar URL must be a valid URL.")]
        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        [MaxLength(500)]
        public string? SocialLinks { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(1000)]
        public string? Interests { get; set; }

        [MaxLength(1000)]
        public string? PersonalityTraits { get; set; }

        [MaxLength(2000)]
        public string? Introduction { get; set; }

        [Url(ErrorMessage = "CV URL must be a valid URL.")]
        [MaxLength(500)]
        public string? CvUrl { get; set; }

        [MaxLength(1000)]
        public string? Note { get; set; }
    }
}
