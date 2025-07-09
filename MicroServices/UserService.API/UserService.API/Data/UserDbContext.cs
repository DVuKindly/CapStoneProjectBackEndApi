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
        public DbSet<LocationMapping> LocationMappings { get; set; }
        public DbSet<Membership> Memberships { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraint
            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.AccountId)
                .IsUnique();

            // 1:1 với UserProfile
            modelBuilder.Entity<StaffProfile>().HasOne(s => s.UserProfile).WithMany().HasForeignKey(s => s.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CoachProfile>().HasOne(c => c.UserProfile).WithMany().HasForeignKey(c => c.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PartnerProfile>().HasOne(p => p.UserProfile).WithMany().HasForeignKey(p => p.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ManagerProfile>().HasOne(m => m.UserProfile).WithMany().HasForeignKey(m => m.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SupplierProfile>().HasOne(s => s.UserProfile).WithMany().HasForeignKey(s => s.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PendingMembershipRequest>().HasOne(p => p.UserProfile).WithMany().HasForeignKey(p => p.AccountId).OnDelete(DeleteBehavior.Restrict);

            // LocationRegion quan hệ
            modelBuilder.Entity<UserProfile>().HasOne(u => u.LocationRegion).WithMany(r => r.UserProfiles).HasForeignKey(u => u.LocationId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<StaffProfile>().HasOne(s => s.LocationRegion).WithMany(r => r.StaffProfiles).HasForeignKey(s => s.LocationId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<PartnerProfile>().HasOne(p => p.LocationRegion).WithMany(r => r.PartnerProfiles).HasForeignKey(p => p.LocationId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ManagerProfile>().HasOne(m => m.LocationRegion).WithMany(r => r.ManagerProfiles).HasForeignKey(m => m.LocationId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<SupplierProfile>().HasOne(s => s.LocationRegion).WithMany().HasForeignKey(s => s.LocationId).OnDelete(DeleteBehavior.SetNull);

            // LocationMapping cấu hình
            modelBuilder.Entity<LocationMapping>()
                .HasKey(x => x.Id);


            // Membership có thể liên kết với PendingMembershipRequest
            modelBuilder.Entity<Membership>()
                .HasOne(m => m.PendingRequest)
                .WithMany()
                .HasForeignKey(m => m.PendingRequestId)
                .OnDelete(DeleteBehavior.SetNull);

            // lịch sử pending 


            modelBuilder.Entity<Membership>().HasOne<UserProfile>()
    .WithMany()
    .HasForeignKey(m => m.AccountId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Membership>().Property(m => m.PackageName).HasMaxLength(200);
            modelBuilder.Entity<Membership>().Property(m => m.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<LocationMapping>()
                .HasIndex(x => new { x.LocationRegionId, x.MembershipLocationId })
                .IsUnique();

            modelBuilder.Entity<LocationMapping>()
                .HasOne(l => l.LocationRegion)
                .WithMany()
                .HasForeignKey(l => l.LocationRegionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LocationMapping>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Config max length
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.CvUrl).HasMaxLength(500);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.Interests).HasMaxLength(1000);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.MessageToStaff).HasMaxLength(2000);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.PaymentMethod).HasMaxLength(50);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.PaymentStatus).HasMaxLength(50);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.PaymentTransactionId).HasMaxLength(100);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.PaymentNote).HasMaxLength(1000);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.PaymentProofUrl).HasMaxLength(1000);
            modelBuilder.Entity<PendingMembershipRequest>().Property(p => p.Amount).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<UserProfile>().Property(p => p.Interests).HasMaxLength(1000);
            modelBuilder.Entity<UserProfile>().Property(p => p.PersonalityTraits).HasMaxLength(1000);
            modelBuilder.Entity<UserProfile>().Property(p => p.Introduction).HasMaxLength(2000);
            modelBuilder.Entity<UserProfile>().Property(p => p.CvUrl).HasMaxLength(500);
             modelBuilder.Entity<LocationRegion>().HasData(
                new LocationRegion
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    Name = "Hà Nội",
                    Description = "Khu vực Hà Nội",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new LocationRegion
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                    Name = "Hải Phòng",
                    Description = "Khu vực Hải Phòng",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new LocationRegion
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                    Name = "Đà Nẵng",
                    Description = "Khu vực Đà Nẵng",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

        }
    }
}
