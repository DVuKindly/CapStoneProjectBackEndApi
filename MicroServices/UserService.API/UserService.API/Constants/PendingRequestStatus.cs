namespace UserService.API.Constants
{
    public static class PendingRequestStatus
    {
        public const string Pending = "Pending";                  // Chờ duyệt
        public const string PendingPayment = "PendingPayment";    // Chờ thanh toán
        public const string Approved = "Approved";                // Đã duyệt
        public const string Rejected = "Rejected";                // Từ chối
        public const string Cancelled = "Cancelled";              // (tuỳ chọn) Đã hủy
    }
}
