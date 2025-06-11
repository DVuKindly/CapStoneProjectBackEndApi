using SharedKernel.DTOsChung.Request;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IMembershipRequestService
    {
        // Người dùng gửi yêu cầu đăng ký membership
        Task<BaseResponse> SubmitRequestAsync(Guid accountId, MembershipRequestDto dto);

        // Staff lấy danh sách các yêu cầu thuộc khu vực của họ (chỉ lấy "Pending")
        Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId);

        // duyệt yêu cầu
        Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffAccountId, ApproveMembershipRequestDto dto);

       //Từ chối yêu cầu
        Task<BaseResponse> RejectMembershipRequestAsync(Guid staffAccountId, RejectMembershipRequestDto dto);




        // Lấy thông tin tóm tắt cho PaymentService
        Task<MembershipRequestSummaryDto?> GetMembershipRequestSummaryAsync(Guid requestId);

        // Dành cho PaymentService gọi về sau khi thanh toán thành công
        Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(Guid requestId);


        Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId, Guid staffAccountId);
        Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId);
    
    }
}
