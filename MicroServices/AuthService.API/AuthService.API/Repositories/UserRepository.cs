using AuthService.API.Data;
using AuthService.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserAuth?> GetByEmailAsync(string email)
        {
            return await _context.AuthUsers
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<Role?> GetRoleByKeyAsync(string roleKey)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleKey == roleKey);
        }


        public async Task<UserAuth?> GetByIdAsync(Guid userId)
        {
            return await _context.AuthUsers.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserAuth?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.AuthUsers.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<UserAuth?> GetByEmailVerificationTokenAsync(string token)
        {
            return await _context.AuthUsers.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
        }

        public async Task<UserAuth?> GetByResetPasswordTokenAsync(string token)
        {
            return await _context.AuthUsers.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
        }
        public async Task<UserAuth?> GetByEmailWithRoleAsync(string email)
        {
            return await _context.AuthUsers
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Email == email);
        }



        public async Task AddAsync(UserAuth user)
        {
            await _context.AuthUsers.AddAsync(user);
        }

        public Task UpdateAsync(UserAuth user)
        {
            _context.AuthUsers.Update(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Lấy user kèm roles
        public async Task<UserAuth?> GetUserWithRolesByAccountIdAsync(Guid accountId)
        {
            return await _context.AuthUsers
           .Include(u => u.UserRoles)
               .ThenInclude(ur => ur.Role)
                   .ThenInclude(r => r.RolePermissions)
                       .ThenInclude(rp => rp.Permission)
           .FirstOrDefaultAsync(u => u.UserId == accountId);
        }

        // Xóa role của user
        public async Task RemoveUserRoleAsync(Guid userId, Guid roleId)
        {
            var userRole = await _context.UserRoles
         .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
                await _context.SaveChangesAsync();
            }

          
        }

        public Task AddUserRoleAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }
    } 
}
