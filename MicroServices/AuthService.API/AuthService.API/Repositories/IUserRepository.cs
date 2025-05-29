using AuthService.API.Entities;

namespace AuthService.API.Repositories
{
    public interface IUserRepository
    {
        Task<UserAuth?> GetByEmailAsync(string email);
        Task<UserAuth?> GetByIdAsync(Guid userId);
        Task<UserAuth?> GetByRefreshTokenAsync(string refreshToken);
        Task<UserAuth?> GetByEmailVerificationTokenAsync(string token);
        Task<UserAuth?> GetByResetPasswordTokenAsync(string token);
        Task<Role?> GetRoleByKeyAsync(string roleKey);

        Task AddAsync(UserAuth user);
        Task UpdateAsync(UserAuth user);
        Task SaveChangesAsync();
    }
}
