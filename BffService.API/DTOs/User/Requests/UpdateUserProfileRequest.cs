using System;

namespace BffService.API.DTOs.Requests
{
    public class UpdateUserProfileRequest
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
    }
}
