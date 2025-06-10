using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class ApproveMembershipRequestDto
    {
        [Required]
        public Guid RequestId { get; set; }

        [MaxLength(1000)]
        public string? StaffNote { get; set; }
    }
}
