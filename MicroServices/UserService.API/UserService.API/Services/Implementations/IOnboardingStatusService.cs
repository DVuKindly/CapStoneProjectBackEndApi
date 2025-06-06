using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Implementations
{
    public interface IOnboardingStatusService
    {
        Task<bool> UpdateStatusAsync(UpdateOnboardingStatusRequest request);
        Task<UserProfileStatusResponse?> GetStatusByAccountIdAsync(Guid accountId);
    }
}
