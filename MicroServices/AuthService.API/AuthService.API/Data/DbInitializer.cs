using AuthService.API.DTOs.Request;
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

            // Nếu đã tồn tại admin thì bỏ qua
            if (await context.AuthUsers.AnyAsync(u => u.Email == adminEmail)) return;

            // Tạo user admin
            var admin = new UserAuth
            {
                UserId = Guid.NewGuid(),
                UserName = "Admin",
                Email = adminEmail,
                EmailVerified = true,
                IsLocked = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            admin.PasswordHash = hasher.HashPassword(admin, adminPassword);

            // Gán role admin
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleKey == "admin");
            if (adminRole == null)
                throw new InvalidOperationException("⚠️ Role 'admin' not found. Please seed roles first.");

            await context.AuthUsers.AddAsync(admin);
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = admin.UserId,
                RoleId = adminRole.RoleId
            });

            await context.SaveChangesAsync();

            // Tạo user profile thông qua UserServiceClient
            var profileInfo = new ProfileInfoRequest
            {
                Note = "SeededBySystem"
            };

            await userServiceClient.CreateUserProfileAsync(
                admin.UserId,
                admin.UserName,
                admin.Email,
                "admin",
                profileInfo
            );

            Console.WriteLine("✅ Admin user and profile seeded successfully.");
        }
    }
}
