using UserService.API.DTOs.Requests;

namespace UserService.API.Services.Interfaces
{
    public interface IStaffProfileService
    {
        Task<bool> CreateAsync(UserProfilePayload payload);
    }

}
