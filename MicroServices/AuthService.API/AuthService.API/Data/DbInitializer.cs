using AuthService.API.DTOs.Request; // 🟢 Import đúng DTO mới
using AuthService.API.Entities;
using AuthService.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            AuthDbContext context,
            IPasswordHasher<UserAuth> hasher,
            IUserServiceClient userServiceClient)
        {
            var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL")
                ?? throw new InvalidOperationException("⚠️ Missing ADMIN_EMAIL in environment variables.");
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD")
                ?? throw new InvalidOperationException("⚠️ Missing ADMIN_PASSWORD in environment variables.");

            // Nếu đã tồn tại super admin thì bỏ qua
            if (await context.AuthUsers.AnyAsync(u => u.Email == adminEmail)) return;

            // Tạo user super admin
            var admin = new UserAuth
            {
                UserId = Guid.NewGuid(),
                UserName = "SuperAdmin",
                Email = adminEmail,
                EmailVerified = true,
                IsLocked = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            admin.PasswordHash = hasher.HashPassword(admin, adminPassword);

            // Gán role super_admin
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleKey == "super_admin");
            if (adminRole == null)
                throw new InvalidOperationException("⚠️ Role 'super_admin' not found. Please seed roles first.");


            await context.AuthUsers.AddAsync(admin);
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = admin.UserId,
                RoleId = adminRole.RoleId
            });
          
            var permissions = await context.Permissions.ToListAsync();

            foreach (var permission in permissions)
            {
                var exists = await context.RolePermissions.AnyAsync(rp =>
                    rp.RoleId == adminRole.RoleId && rp.PermissionId == permission.PermissionId);

                if (!exists)
                {
                    await context.RolePermissions.AddAsync(new RolePermission
                    {
                        RoleId = adminRole.RoleId,
                        PermissionId = permission.PermissionId
                    });
                }
            }

            await context.SaveChangesAsync();

           
            var profilePayload = new UserProfilePayload
            {
                AccountId = admin.UserId,
                FullName = admin.UserName,
                Email = admin.Email,
                RoleType = "super_admin",
                Note = "SeededBySystem",
                OnboardingStatus = "AdminSystem"
            };

          
            await userServiceClient.CreateUserProfileAsync(
                admin.UserId,
                admin.UserName,
                admin.Email,
                "super_admin",
                profilePayload
            );

            Console.WriteLine("✅ SuperAdmin user and profile seeded successfully.");
        }
    }
}
