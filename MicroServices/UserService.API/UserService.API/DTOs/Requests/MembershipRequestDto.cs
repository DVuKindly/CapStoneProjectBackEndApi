using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Requests
{
    public class MembershipRequestDto
    {
        [Required(ErrorMessage = "Gói đăng ký là bắt buộc.")]
        public Guid PackageId { get; set; }

        public string PackageType { get; set; } = "basic";
        public DateTime? SelectedStartDate { get; set; } // ngày user chọn

        [MaxLength(2000)]
        public string? MessageToStaff { get; set; }

        /// <summary>
        /// Chỉ cần khi gói Basic thanh toán trực tiếp (VerifyBuy = true)
        /// </summary>
        public string? RedirectUrl { get; set; }
    }
}
