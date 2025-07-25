﻿using System.ComponentModel.DataAnnotations;
using AuthService.API.DTOs.Interfaces;

namespace AuthService.API.DTOs.Request
{
    public class RegisterBySuperAdminRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;


       public Guid LocationId { get; set; }

        public bool SkipEmailVerification { get; set; } = true;

     
    }
}
