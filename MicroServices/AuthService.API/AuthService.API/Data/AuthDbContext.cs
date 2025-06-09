using AuthService.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAuth> AuthUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuth>().ToTable("AuthUsers");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Permission>().ToTable("Permissions");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions");
            modelBuilder.Entity<RolePermission>().ToTable("RolePermissions");

            // ✅ Không có HasMaxLength cho Guid
            // ❌ KHÔNG dùng HasMaxLength cho LocationId nữa

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

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

         
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = SystemRoles.SuperAdmin, RoleKey = "super_admin", RoleName = "Quản trị hệ thống cấp cao", Description = "Quản lý toàn bộ hệ thống, toàn quyền" },
                new Role { RoleId = SystemRoles.Admin, RoleKey = "admin", RoleName = "Admin khu vực", Description = "Quản lý hệ thống theo khu vực" },
                new Role { RoleId = SystemRoles.Manager, RoleKey = "manager", RoleName = "Quản lý nội dung", Description = "Duyệt nội dung từ các staff" },
                new Role { RoleId = SystemRoles.StaffOnboarding, RoleKey = "staff_onboarding", RoleName = "Nhân viên onboarding", Description = "Duyệt hồ sơ đăng ký, tạo package" },
                new Role { RoleId = SystemRoles.StaffService, RoleKey = "staff_service", RoleName = "Nhân viên vận hành", Description = "Duyệt nội dung và dịch vụ từ đối tác" },
                new Role { RoleId = SystemRoles.StaffContent, RoleKey = "staff_content", RoleName = "Nhân viên nội dung", Description = "Tạo và gửi nội dung chờ duyệt" },
                new Role { RoleId = SystemRoles.User, RoleKey = "user", RoleName = "Khách đăng nhập", Description = "Chưa đăng ký gói, chỉ xem công khai" },
                new Role { RoleId = SystemRoles.Member, RoleKey = "member", RoleName = "Thành viên", Description = "Đã thanh toán gói membership, truy cập nội dung" },
                new Role { RoleId = SystemRoles.Partner, RoleKey = "partner", RoleName = "Đối tác nội dung", Description = "Chủ động đăng nội dung khóa học/sự kiện" },
                new Role { RoleId = SystemRoles.Coaching, RoleKey = "coaching", RoleName = "Hướng dẫn viên", Description = "Giảng viên, mentor lớp học" },
                new Role { RoleId = SystemRoles.Supplier, RoleKey = "supplier", RoleName = "Nhà cung cấp nội dung", Description = "Cung cấp nội dung, tính hoa hồng" }
            );
        }
    }
}
