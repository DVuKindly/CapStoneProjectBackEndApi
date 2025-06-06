using UserService.API.DTOs.Requests;

using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Implementations
{
    public interface IUserProfileService
    {
        Task<bool> CreateAsync(CreateUserProfileRequest request);
        Task<bool> UpdateAsync(UpdateUserProfileRequest request);
        Task<UserProfileResponse?> GetByAccountIdAsync(Guid accountId);
        Task<UserProfileStatusResponse?> GetStatusAsync(Guid accountId);
        Task<CheckCanPromoteResponse> CheckCanPromoteAsync(Guid accountId);
        Task<List<UserProfileStatusResponse>> GetIncompleteProfilesAsync();
    }
}
