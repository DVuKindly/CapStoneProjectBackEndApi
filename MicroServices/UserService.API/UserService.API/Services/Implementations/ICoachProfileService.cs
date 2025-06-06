using UserService.API.DTOs.Requests;

namespace UserService.API.Services.Implementations
{
    public interface ICoachProfileService
    {
        Task<bool> CreateAsync(CreateCoachProfileRequest request);
    }
}
