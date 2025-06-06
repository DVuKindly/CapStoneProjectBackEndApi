using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Implementations
{
    public interface IStaffProfileService
    {
        Task<bool> CreateAsync(CreateStaffProfileRequest request);
        Task<StaffProfileResponse?> GetByAccountIdAsync(Guid accountId);
        Task<bool> UpdateAsync(UpdateStaffProfileRequest request);
        Task<List<StaffProfileResponse>> GetAllAsync();
    }
}
