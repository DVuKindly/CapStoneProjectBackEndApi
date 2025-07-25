﻿using AuthService.API.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.COACH
{
    public class CoachProfileInfoRequest : IProfileInfoRequest
    {
        [Required, MaxLength(100)]
        public string CoachType { get; set; } = null!;

        [MaxLength(255)]
        public string? Specialty { get; set; }

        [MaxLength(255)]
        public string? ModuleInCharge { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        [Range(0, 100)]
        public int? ExperienceYears { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        [MaxLength(500)]
        public string? Certifications { get; set; }

        [MaxLength(255)]
        public string? LinkedInUrl { get; set; }
    }
}
