using System;
using System.Threading.Tasks;
using SharedKernel.DTOsChung;

namespace PaymentService.API.Services.Interfaces
{
    public interface IUserServiceClient
    {
        // Thông báo thanh toán thành công, bỏ qua IsDirectMembership (tính sau)
        Task<bool> NotifyPaymentSuccessAsync(Guid paymentRequestId, Guid? membershipRequestId = null);

        // Đánh dấu yêu cầu thanh toán đã thành công (dùng chung cho tất cả các loại)
        Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto);
    }
}
