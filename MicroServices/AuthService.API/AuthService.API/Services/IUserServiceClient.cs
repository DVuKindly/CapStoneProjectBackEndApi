namespace AuthService.API.Services
{
    public interface IUserServiceClient
    {
        Task<bool> CreateUserProfileAsync(Guid accountId, string fullName, string email);
    }

}
