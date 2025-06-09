using AuthService.API.DTOs.AdminCreate;
using System;
using System.Threading.Tasks;

namespace AuthService.API.Services
{
    public interface IUserServiceClient
    {
        Task CreateUserProfileAsync(
            Guid userId,
            string userName,
            string email,
            string roleType = "user",
            object? profileInfo = null
        );
        Task<List<LocationDto>> GetLocationsAsync();
        Task<bool> IsValidLocationAsync(Guid locationId);
    }
}
