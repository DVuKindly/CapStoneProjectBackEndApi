using AuthService.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<UserAuth> AuthUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAuth>().ToTable("AuthUsers");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Permission>().ToTable("Permissions");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermissions");

            modelBuilder.Entity<UserAuth>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<UserAuth>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<UserAuth>()
                .Property(u => u.EmailVerified)
                .HasDefaultValue(false);

            modelBuilder.Entity<UserAuth>()
                .Property(u => u.IsLocked)
                .HasDefaultValue(false);

            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserPermission>().HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);

            modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = SystemRoles.SuperAdmin, RoleKey = "super_admin", RoleName = "Super Admin", Description = "Toàn quyền, tạo admin khu vực" },
                new Role { RoleId = SystemRoles.Admin, RoleKey = "admin", RoleName = "Admin khu vực", Description = "Tạo các role khác trong khu vực" },
                new Role { RoleId = SystemRoles.Manager, RoleKey = "manager", RoleName = "Manager", Description = "Duyệt nội dung và package" },
                new Role { RoleId = SystemRoles.StaffOnboarding, RoleKey = "staff_onboarding", RoleName = "Staff Onboarding", Description = "Tạo package, duyệt user-member" },
                new Role { RoleId = SystemRoles.StaffService, RoleKey = "staff_service", RoleName = "Staff Service", Description = "Tạo event hoạt động Co-Living" },
                new Role { RoleId = SystemRoles.StaffContent, RoleKey = "staff_content", RoleName = "Staff Content", Description = "Đăng nội dung" },
                new Role { RoleId = SystemRoles.User, RoleKey = "user", RoleName = "User", Description = "Khách đăng nhập" },
                new Role { RoleId = SystemRoles.Member, RoleKey = "member", RoleName = "Member", Description = "Thành viên trả phí" },
                new Role { RoleId = SystemRoles.Partner, RoleKey = "partner", RoleName = "Partner", Description = "Đối tác nội dung" },
                new Role { RoleId = SystemRoles.Coaching, RoleKey = "coaching", RoleName = "Coaching", Description = "Hướng dẫn viên / mentor" },
                new Role { RoleId = SystemRoles.Supplier, RoleKey = "supplier", RoleName = "Supplier", Description = "Nhà cung cấp nội dung" }
            );

            modelBuilder.Entity<Permission>().HasData(
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), PermissionKey = "assign_role", Description = "Gán role" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"), PermissionKey = "assign_permission_to_role", Description = "Gán quyền cho role" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0003"), PermissionKey = "assign_permission_to_user", Description = "Gán quyền cho user" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004"), PermissionKey = "access_admin_dashboard", Description = "Truy cập dashboard admin" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005"), PermissionKey = "create_supplier", Description = "Tạo nhà cung cấp" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006"), PermissionKey = "create_partner", Description = "Tạo đối tác" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"), PermissionKey = "create_manager", Description = "Tạo Manager" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"), PermissionKey = "create_staff_onboarding", Description = "Tạo Staff Onboarding" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"), PermissionKey = "create_staff_service", Description = "Tạo Staff Service" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"), PermissionKey = "create_staff_content", Description = "Tạo Staff Content" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"), PermissionKey = "create_coach", Description = "Tạo Coach" },
       new Permission { PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0012"), PermissionKey = "create_admin", Description = "Tạo Admin Khu vực" }

   );


            var superAdminId = SystemRoles.SuperAdmin;
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0003") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010") },
                new RolePermission { RoleId = superAdminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011") }
            );
            var adminId = SystemRoles.Admin;
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004") }, // access dashboard
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005") }, // create supplier
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006") }, // create partner
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007") }, // create manager
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008") }, // create staff onboarding
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009") }, // create staff service
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010") }, // create staff content
                new RolePermission { RoleId = adminId, PermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011") }  // create coach
            );

        }
    }
}
