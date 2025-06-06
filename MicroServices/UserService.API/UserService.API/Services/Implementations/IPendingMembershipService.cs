using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IPendingMembershipService
    {
        // 1. User gửi yêu cầu chọn gói → tạo pending request
        Task<bool> CreateRequestAsync(CreatePendingMembershipRequest request);

        // 2. Staff xem tất cả hồ sơ pending theo location (hoặc tất cả)
        Task<List<PendingMembershipRequestResponse>> GetAllPendingAsync();
        Task<List<PendingMembershipRequestResponse>> GetPendingByLocationAsync(string location);

        // 3. Staff duyệt hồ sơ → chuyển trạng thái sang ApprovedAwaitingPayment
        Task<bool> ApproveAsync(ApprovePendingMembershipRequest request);

        // 4. Staff từ chối hồ sơ
        Task<bool> RejectAsync(RejectPendingMembershipRequest request);

        // 5. Khi user thanh toán thành công → gọi để cập nhật trạng thái sang PaymentConfirmed
        Task<bool> ConfirmPaymentAsync(Guid accountId);

        // 6. (Optional) Kiểm tra trạng thái request
        Task<PendingMembershipRequestResponse?> GetByAccountIdAsync(Guid accountId);
    }
}
