using AuthService.API.Data;
using AuthService.API.DTOs.Responses;
using AuthService.API.Entities;
using AuthService.API.Services;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;
        private readonly IUserServiceClient _userServiceClient;

        public UserRepository(AuthDbContext context, IUserServiceClient userServiceClient)
        {
            _context = context;
            _userServiceClient = userServiceClient;
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
        public async Task<List<UserAuth>> GetAccountsByRoleKeysAsync(string[] roleKeys)
        {
            return await _context.AuthUsers
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => roleKeys.Contains(ur.Role.RoleKey)))
                .ToListAsync();
        }



        public async Task AddUserRoleAsync(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
        }

        public async Task<List<UserAuth>> GetAllUsersWithRolesAsync()
        {
            return await _context.AuthUsers
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

   
      








    }
}
