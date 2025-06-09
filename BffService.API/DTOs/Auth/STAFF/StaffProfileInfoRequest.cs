using BffService.API.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.STAFF
{
    public class StaffProfileInfoRequest : IProfileInfoRequest
    {
        [Required, MaxLength(100)]
        public string Phone { get; set; } = null!;

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateTime? DOB { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        [MaxLength(255)]
        public string? Note { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        [MaxLength(50)]
        public string? Level { get; set; }
    }
}
