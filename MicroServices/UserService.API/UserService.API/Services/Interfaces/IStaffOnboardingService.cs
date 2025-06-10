using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IStaffOnboardingService
    {
        Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffId, ApproveMembershipRequestDto dto);
    }
}
