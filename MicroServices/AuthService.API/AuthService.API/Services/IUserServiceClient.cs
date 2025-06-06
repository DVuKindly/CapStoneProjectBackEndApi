using AuthService.API.DTOs.Request;

namespace AuthService.API.Services
{
    public interface IUserServiceClient
    {
        Task CreateUserProfileAsync(
            Guid userId,
            string userName,
            string email,
            string roleType = "User",
            ProfileInfoRequest? profileInfo = null
        );
    }
}
