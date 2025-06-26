using SharedKernel.DTOsChung.Request;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<List<MembershipResponseDto>> GetUserMembershipsAsync(Guid accountId);
        Task CheckAndDowngradeExpiredMembershipsAsync();
        Task<BaseResponse> CreateMembershipAsync(CreateMembershipDto dto);
        Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(MarkPaidRequestDto dto);
        Task<MembershipRequestSummaryDto?> GetMembershipSummaryAsync(Guid membershipId);
        Task<DurationDto?> GetPlanDurationAsync(Guid planId, string planType);
    }
}
