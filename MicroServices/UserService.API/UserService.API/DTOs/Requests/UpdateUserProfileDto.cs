using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class UpdateUserProfileDto
    {
        [Required(ErrorMessage = "FullName là bắt buộc.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "FullName phải từ {2} đến {1} ký tự.")]
        public string FullName { get; set; } = null!;

        [Phone(ErrorMessage = "Phone không đúng định dạng.")]
        [StringLength(15, ErrorMessage = "Phone tối đa {1} ký tự.")]
        public string? Phone { get; set; }

        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender phải là 'Male', 'Female' hoặc 'Other'.")]
        public string? Gender { get; set; }

        [DataType(DataType.Date, ErrorMessage = "DOB phải là ngày hợp lệ.")]
        public DateTime? DOB { get; set; }

        [Url(ErrorMessage = "AvatarUrl phải là một URL hợp lệ.")]
        [StringLength(500, ErrorMessage = "AvatarUrl tối đa {1} ký tự.")]
        public string? AvatarUrl { get; set; }

        [StringLength(500, ErrorMessage = "SocialLinks tối đa {1} ký tự.")]
        public string? SocialLinks { get; set; }

        [StringLength(255, ErrorMessage = "Address tối đa {1} ký tự.")]
        public string? Address { get; set; }

        [StringLength(1000, ErrorMessage = "Sở thích tối đa {1} ký tự.")]
        public string? Interests { get; set; }

        [StringLength(1000, ErrorMessage = "Tính cách tối đa {1} ký tự.")]
        public string? PersonalityTraits { get; set; }

        [StringLength(2000, ErrorMessage = "Giới thiệu bản thân tối đa {1} ký tự.")]
        public string? Introduction { get; set; }

        [Url(ErrorMessage = "CvUrl phải là một URL hợp lệ.")]
        [StringLength(500, ErrorMessage = "CvUrl tối đa {1} ký tự.")]
        public string? CvUrl { get; set; }

        [StringLength(1000, ErrorMessage = "Ghi chú tối đa {1} ký tự.")]
        public string? Note { get; set; }
    }
}
