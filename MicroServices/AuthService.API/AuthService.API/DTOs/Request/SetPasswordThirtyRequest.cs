﻿using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs.Request
{
    public class SetPasswordThirtyRequest
    {
        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public string NewPassword { get; set; } = null!;
    }
}
