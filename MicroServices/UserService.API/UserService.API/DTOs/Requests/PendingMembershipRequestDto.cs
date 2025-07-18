namespace UserService.API.DTOs.Requests
{
    public class PendingMembershipRequestDto
    {
        public Guid RequestId { get; set; }
        public string? FullName { get; set; }

        // Gói đã yêu cầu
        public string? RequestedPackageName { get; set; }
        public string? PackageType { get; set; } // basic / combo
        public decimal? Amount { get; set; }
        public DateTime? ExpireAt { get; set; }
        public DateTime? StartDate { get; set; }
        // Trạng thái yêu cầu & thanh toán
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; } // Paid / Unpaid
        public string? PaymentMethod { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string? StaffNote { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public Guid? PackageId { get; set; } 
        // Thông tin thêm
        public string? MessageToStaff { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LocationName { get; set; }

        // Từ hồ sơ cá nhân
        public string? Interests { get; set; }
        public string? PersonalityTraits { get; set; }
        public string? Introduction { get; set; }
        public string? CvUrl { get; set; }

        public ExtendedProfileDto? ExtendedProfile { get; set; }

        // ✅ BỔ SUNG 2 THUỘC TÍNH MỚI
        public bool? RequireBooking { get; set; }
        public Guid? RoomInstanceId { get; set; }
    }

    public class ExtendedProfileDto
    {
        public string? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string? AvatarUrl { get; set; }
        public string? SocialLinks { get; set; }
        public string? Address { get; set; }
        public string? RoleType { get; set; }
    }
}
