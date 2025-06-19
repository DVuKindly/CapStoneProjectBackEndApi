using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class MembershipRequestDto
    {
        [Required(ErrorMessage = "Gói đăng ký là bắt buộc.")]
        public Guid PackageId { get; set; }

        [MaxLength(2000)]
        public string? MessageToStaff { get; set; }
    }
        
}
