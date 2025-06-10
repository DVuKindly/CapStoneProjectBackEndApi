using System.ComponentModel.DataAnnotations;

namespace BffService.API.DTOs.Requests
{
    public class MembershipRequestDto
    {
        [Required(ErrorMessage = "Gói đăng ký là bắt buộc.")]
        public Guid PackageId { get; set; }

        [Required(ErrorMessage = "Khu vực là bắt buộc.")]
        public Guid LocationId { get; set; }

        [Required,MaxLength(255)]
        public string? RequestedPackageName { get; set; }

        [MaxLength(2000)]
        public string? MessageToStaff { get; set; }
    }

}
