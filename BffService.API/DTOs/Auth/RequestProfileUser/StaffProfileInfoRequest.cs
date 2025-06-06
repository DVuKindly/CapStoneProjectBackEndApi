using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Auth.RequestProfileUser
{
    public class StaffProfileInfoRequest
    {
        [Required, MaxLength(100)]
        public string? Phone { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateTime? DOB { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(255)]
        public string? Note { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        [MaxLength(50)]
        public string? Level { get; set; }
    }
}
