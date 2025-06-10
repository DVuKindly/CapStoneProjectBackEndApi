using Microsoft.EntityFrameworkCore;
using UserService.API.Entities;

namespace UserService.API.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<CoachProfile> CoachProfiles { get; set; }
        public DbSet<ManagerProfile> ManagerProfiles { get; set; }
        public DbSet<SupplierProfile> SupplierProfiles { get; set; }
        public DbSet<PartnerProfile> PartnerProfiles { get; set; }
        public DbSet<LocationRegion> LocationRegions { get; set; }
        public DbSet<PendingMembershipRequest> PendingMembershipRequests { get; set; }
        public DbSet<PendingThirdPartyRequest> PendingThirdPartyRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique AccountId trong UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.AccountId)
                .IsUnique();

            // 1:1 giữa các Profile và UserProfile
            modelBuilder.Entity<StaffProfile>()
                .HasOne(s => s.UserProfile)
                .WithMany()
                .HasForeignKey(s => s.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CoachProfile>()
                .HasOne(c => c.UserProfile)
                .WithMany()
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PartnerProfile>()
                .HasOne(p => p.UserProfile)
                .WithMany()
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ManagerProfile>()
                .HasOne(m => m.UserProfile)
                .WithMany()
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupplierProfile>()
                .HasOne(s => s.UserProfile)
                .WithMany()
                .HasForeignKey(s => s.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PendingMembershipRequest>()
                .HasOne(p => p.UserProfile)
                .WithMany()
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1:N với LocationRegion (set null khi xóa region)
            modelBuilder.Entity<UserProfile>()
                .HasOne(u => u.LocationRegion)
                .WithMany(r => r.UserProfiles)
                .HasForeignKey(u => u.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StaffProfile>()
                .HasOne(s => s.LocationRegion)
                .WithMany(r => r.StaffProfiles)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PartnerProfile>()
                .HasOne(p => p.LocationRegion)
                .WithMany(r => r.PartnerProfiles)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ManagerProfile>()
                .HasOne(m => m.LocationRegion)
                .WithMany(r => r.ManagerProfiles)
                .HasForeignKey(m => m.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PendingMembershipRequest>()
                .HasOne(p => p.LocationRegion)
                .WithMany(r => r.PendingMembershipRequests)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PendingThirdPartyRequest>()
                .HasOne(p => p.LocationRegion)
                .WithMany(r => r.PendingThirdPartyRequests)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<PendingMembershipRequest>()
    .Property(p => p.CvUrl).HasMaxLength(500);

            modelBuilder.Entity<PendingMembershipRequest>()
                .Property(p => p.Interests).HasMaxLength(1000);

            modelBuilder.Entity<PendingMembershipRequest>()
                .Property(p => p.MessageToStaff).HasMaxLength(2000);



            modelBuilder.Entity<UserProfile>()
    .Property(p => p.Interests)
    .HasMaxLength(1000);

            modelBuilder.Entity<UserProfile>()
                .Property(p => p.PersonalityTraits)
                .HasMaxLength(1000);

            modelBuilder.Entity<UserProfile>()
                .Property(p => p.Introduction)
                .HasMaxLength(2000);

            modelBuilder.Entity<UserProfile>()
                .Property(p => p.CvUrl)
                .HasMaxLength(500);




            modelBuilder.Entity<PendingMembershipRequest>()
    .Property(p => p.PaymentMethod).HasMaxLength(50);

            modelBuilder.Entity<PendingMembershipRequest>()
                .Property(p => p.PaymentStatus).HasMaxLength(50);

            modelBuilder.Entity<PendingMembershipRequest>()
                .Property(p => p.PaymentTransactionId).HasMaxLength(100);

            modelBuilder.Entity<PendingMembershipRequest>()
                .Property(p => p.PaymentNote).HasMaxLength(1000);

            modelBuilder.Entity<PendingMembershipRequest>()
                .Property(p => p.PaymentProofUrl).HasMaxLength(1000);




            // Cấu hình đơn giản cho SupplierProfile → LocationRegion, không cần nav collection ở LocationRegion
            modelBuilder.Entity<SupplierProfile>()
                .HasOne(s => s.LocationRegion)
                .WithMany() // không chỉ định ICollection<SupplierProfile> trong LocationRegion
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed data cho LocationRegion
            modelBuilder.Entity<LocationRegion>().HasData(
                new LocationRegion
                {
                    Id = Guid.Parse("9f38b827-4e1a-4a6e-b8c5-5ff6b759a2a1"),
                    Name = "Hà Nội",
                    Description = "Khu vực Hà Nội",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new LocationRegion
                {
                    Id = Guid.Parse("5a418674-9e47-4d19-b827-1e8e2b25c324"),
                    Name = "Hải Phòng",
                    Description = "Khu vực Hải Phòng",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new LocationRegion
                {
                    Id = Guid.Parse("f0b2b2d9-5e77-4c7e-a601-2e3b9b740e0c"),
                    Name = "Đà Nẵng",
                    Description = "Khu vực Đà Nẵng",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}
