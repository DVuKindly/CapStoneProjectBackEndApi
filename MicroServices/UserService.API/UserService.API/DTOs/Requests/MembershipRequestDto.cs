using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class MembershipRequestDto
    {
        [Required(ErrorMessage = "Gói đăng ký là bắt buộc.")]
        public Guid PackageId { get; set; }

        public string PackageType { get; set; } = "basic";

        public DateTime? SelectedStartDate { get; set; }

        public bool RequireBooking { get; set; } = false;

        
        public Guid? RoomInstanceId { get; set; }

        [MaxLength(2000)]
        public string? MessageToStaff { get; set; }

    
        public string? RedirectUrl { get; set; }
    }
}
