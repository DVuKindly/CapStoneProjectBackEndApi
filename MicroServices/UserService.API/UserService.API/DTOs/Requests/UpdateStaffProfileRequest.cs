using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class UpdateStaffProfileRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Level { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public Guid ManagerId { get; set; }
        public bool IsActive { get; set; }
    }
}
