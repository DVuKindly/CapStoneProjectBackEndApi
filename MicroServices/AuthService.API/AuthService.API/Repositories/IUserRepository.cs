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
        Task<UserAuth?> GetByEmailWithRoleAsync(string email);

        // MỚI: Lấy user cùng roles (để cập nhật role)
        Task<UserAuth?> GetUserWithRolesByAccountIdAsync(Guid accountId);

        // MỚI: Xóa một role của user
        Task RemoveUserRoleAsync(Guid userId, Guid roleId);

        // MỚI: Thêm role cho user
        Task AddUserRoleAsync(UserRole userRole);
    }
}
