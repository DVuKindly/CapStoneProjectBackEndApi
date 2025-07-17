using SharedKernel.DTOsChung.Request;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;

namespace UserService.API.Services.Interfaces
{
    public interface IMembershipRequestService
    {
        /// <summary>
        /// Gửi yêu cầu đăng ký gói membership (basic hoặc combo).
        /// </summary>
        Task<BaseResponse> SubmitRequestAsync(Guid accountId, MembershipRequestDto dto);

        /// <summary>
        /// Lấy danh sách yêu cầu đang chờ xử lý (dành cho Staff theo khu vực).
        /// </summary>
        Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId);

        /// <summary>
        /// Phê duyệt yêu cầu đăng ký membership (từ staff).
        /// </summary>
        Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffAccountId, ApproveMembershipRequestDto dto);

        /// <summary>
        /// Từ chối yêu cầu đăng ký membership (từ staff).
        /// </summary>
        Task<BaseResponse> RejectMembershipRequestAsync(Guid staffAccountId, RejectMembershipRequestDto dto);

        /// <summary>
        /// Lấy thông tin tóm tắt yêu cầu membership để hiển thị thanh toán.
        /// </summary>
        Task<MembershipRequestSummaryDto?> GetMembershipRequestSummaryAsync(Guid requestId);


        /// <summary>
        /// Đánh dấu một yêu cầu đã thanh toán và xử lý quyền tương ứng.
        /// </summary>
        Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(MarkPaidRequestDto dto);



        /// <summary>
        /// Lấy chi tiết một yêu cầu cụ thể (dành cho staff theo khu vực).
        /// </summary>
        Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId, Guid staffAccountId);

        /// <summary>
        /// Lịch sử yêu cầu membership của người dùng.
        /// </summary>
        Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId);

        Task<List<PendingMembershipRequestDto>> GetAllRequestsForStaffLocationAsync(Guid staffAccountId);

        bool IsUserProfileCompleted(UserProfile user);

        // cancel request 
        Task<BaseResponse> CancelRequestAsync(Guid accountId, Guid requestId);
    }
}
