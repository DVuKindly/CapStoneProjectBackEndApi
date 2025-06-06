using UserService.API.DTOs.Requests;

namespace UserService.API.Services.Implementations
{
    public interface IPartnerProfileService
    {
        Task<bool> CreateAsync(CreatePartnerProfileRequest request);
    }
}
