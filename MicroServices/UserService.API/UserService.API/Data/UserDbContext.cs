using Microsoft.EntityFrameworkCore;
using UserService.API.Entities;

namespace UserService.API.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<CoachProfile> CoachProfiles { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<PartnerProfile> PartnerProfiles { get; set; }
        public DbSet<PendingMembershipRequest> PendingMembershipRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("UserDBCapStone");

            // USER PROFILE
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfiles");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AccountId).IsUnique();

                entity.Property(e => e.AccountId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.DOB);
                entity.Property(e => e.AvatarUrl).HasMaxLength(500);
                entity.Property(e => e.SocialLinks).HasMaxLength(1000);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.RoleType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsCompleted).IsRequired();
                entity.Property(e => e.IsVerifiedByAdmin).IsRequired();
                entity.Property(e => e.OnboardingStatus).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });

            // COACH PROFILE
            modelBuilder.Entity<CoachProfile>(entity =>
            {
                entity.ToTable("CoachProfiles");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CoachType).HasMaxLength(100);
                entity.Property(e => e.Specialty).HasMaxLength(255);
                entity.Property(e => e.ModuleInCharge).HasMaxLength(255);
                entity.Property(e => e.Region).HasMaxLength(100);

                entity.HasOne(e => e.UserProfile)
                      .WithMany(u => u.CoachProfiles)
                      .HasForeignKey(e => e.AccountId)
                      .HasPrincipalKey(u => u.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // STAFF PROFILE
            modelBuilder.Entity<StaffProfile>(entity =>
            {
                entity.ToTable("StaffProfiles");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.StaffGroup).HasMaxLength(100);

                entity.HasOne(e => e.UserProfile)
                      .WithMany(u => u.StaffProfiles)
                      .HasForeignKey(e => e.AccountId)
                      .HasPrincipalKey(u => u.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PARTNER PROFILE
            modelBuilder.Entity<PartnerProfile>(entity =>
            {
                entity.ToTable("PartnerProfiles");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.OrganizationName).HasMaxLength(255);
                entity.Property(e => e.PartnerType).HasMaxLength(100);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.ContractUrl).HasMaxLength(500);
                entity.Property(e => e.IsActivated).IsRequired();
                entity.Property(e => e.ActivatedAt);
                entity.Property(e => e.CreatedByAdminId).HasMaxLength(100);

                entity.HasOne(e => e.UserProfile)
                      .WithMany(u => u.PartnerProfiles)
                      .HasForeignKey(e => e.AccountId)
                      .HasPrincipalKey(u => u.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PENDING MEMBERSHIP REQUEST
            modelBuilder.Entity<PendingMembershipRequest>(entity =>
            {
                entity.ToTable("PendingMembershipRequests");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AccountId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.RequestedPackageName).HasMaxLength(255);
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.StaffNote).HasMaxLength(1000);
                entity.Property(e => e.ApprovedBy).HasMaxLength(100);
                entity.Property(e => e.ApprovedAt);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.UserProfile)
                      .WithMany(u => u.PendingMembershipRequests)
                      .HasForeignKey(e => e.AccountId)
                      .HasPrincipalKey(u => u.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
