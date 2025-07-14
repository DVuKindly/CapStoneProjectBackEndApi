using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class UpdateUserProfileDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Full name must be between {2} and {1} characters.")]
        public string FullName { get; set; } = null!;

        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must start with 0 and contain exactly 10 digits.")]
        public string? Phone { get; set; }

        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be 'Male', 'Female' or 'Other'.")]
        public string? Gender { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Date of birth must be a valid date.")]
        public DateTime? DOB { get; set; }
        public List<Guid>? InterestIds { get; set; }
        public List<Guid>? PersonalityTraitIds { get; set; }

        [Url(ErrorMessage = "Avatar URL must be a valid URL.")]
        [MaxLength(500, ErrorMessage = "Avatar URL must not exceed {1} characters.")]
        public string? AvatarUrl { get; set; }

        [MaxLength(500, ErrorMessage = "Social links must not exceed {1} characters.")]
        public string? SocialLinks { get; set; }

        [MaxLength(255, ErrorMessage = "Address must not exceed {1} characters.")]
        public string? Address { get; set; }

        [MaxLength(1000, ErrorMessage = "Interests must not exceed {1} characters.")]
        public string? Interests { get; set; }

        [MaxLength(1000, ErrorMessage = "Personality traits must not exceed {1} characters.")]
        public string? PersonalityTraits { get; set; }

        [MaxLength(2000, ErrorMessage = "Introduction must not exceed {1} characters.")]
        public string? Introduction { get; set; }

        [Url(ErrorMessage = "CV URL must be a valid URL.")]
        [MaxLength(500, ErrorMessage = "CV URL must not exceed {1} characters.")]
        public string? CvUrl { get; set; }

        [MaxLength(1000, ErrorMessage = "Note must not exceed {1} characters.")]
        public string? Note { get; set; }
    }
}
