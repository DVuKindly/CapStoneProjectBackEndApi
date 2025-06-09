using UserService.API.DTOs.Requests;

namespace UserService.API.Services.Interfaces
{
    public interface IPartnerProfileService
    {
        Task<bool> CreateAsync(UserProfilePayload payload);
    }
}
