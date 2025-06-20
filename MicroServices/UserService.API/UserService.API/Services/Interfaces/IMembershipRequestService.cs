using SharedKernel.DTOsChung.Request;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IMembershipRequestService
    {
   
        Task<BaseResponse> SubmitRequestAsync(Guid accountId, MembershipRequestDto dto);

       
        Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId);

     
        Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffAccountId, ApproveMembershipRequestDto dto);

     
        Task<BaseResponse> RejectMembershipRequestAsync(Guid staffAccountId, RejectMembershipRequestDto dto);




      
        Task<MembershipRequestSummaryDto?> GetMembershipRequestSummaryAsync(Guid requestId);

    
        Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(MarkPaidRequestDto dto);


        Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId, Guid staffAccountId);
        Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId);
    
    }
}
