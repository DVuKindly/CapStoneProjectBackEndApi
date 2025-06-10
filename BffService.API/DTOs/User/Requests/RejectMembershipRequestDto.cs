using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Requests
{
    public class RejectMembershipRequestDto
    {
        [Required]
        public Guid RequestId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Reason { get; set; } = string.Empty;
    }

}
