using MembershipService.API.Entities;
using MembershipService.API.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Data
{
    public class MembershipDbContext : DbContext
    {
        public MembershipDbContext(DbContextOptions<MembershipDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<Ecosystem> Ecosystems { get; set; }
        public DbSet<NextUService> NextUServices { get; set; }
        public DbSet<AccommodationOption> AccommodationOptions { get; set; }
        public DbSet<RoomInstance> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EntitlementRule> EntitlementRules { get; set; }
        public DbSet<Media> MediaGallery { get; set; }
        public DbSet<PackageDuration> PackageDurations { get; set; }
        public DbSet<ComboPlanDuration> ComboPlanDurations { get; set; }
        public DbSet<BasicPlanType> BasicPlanTypes { get; set; }
        public DbSet<BasicPlan> BasicPlans { get; set; }
        public DbSet<BasicPlanCategory> BasicPlanCategories { get; set; }
        public DbSet<BasicPlanLevel> BasicPlanLevels { get; set; }
        public DbSet<PlanTargetAudience> PlanTargetAudiences { get; set; }
        public DbSet<BasicPlanEntitlement> BasicPlanEntitlements { get; set; }
        public DbSet<BasicPlanRoom> BasicPlanRooms { get; set; }
        public DbSet<ComboPlan> ComboPlans { get; set; }
        public DbSet<ComboPlanBasic> ComboPlanBasics { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PackageLevel> PackageLevels { get; set; }
        public DbSet<Entities.Membership> Memberships { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Media>()
                .Property(m => m.Url)
                .IsRequired();

            modelBuilder.Entity<Media>()
                .Property(m => m.Type)
                .IsRequired();

            // Ecosystem - Service
            modelBuilder.Entity<NextUService>()
                .HasOne(s => s.Ecosystem)
                .WithMany(e => e.NextUServices)
                .HasForeignKey(s => s.EcosystemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Location - Service
            modelBuilder.Entity<NextUService>()
                .HasOne(s => s.Location)
                .WithMany(e => e.NextUServices)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Location - AccommodationOption
            modelBuilder.Entity<AccommodationOption>()
                .HasOne(s => s.Location)
                .WithMany(e => e.AccommodationOptions)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Location - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.Location)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Location - ComboPlan
            modelBuilder.Entity<ComboPlan>()
                .HasOne(s => s.Location)
                .WithMany(e => e.ComboPlans)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlanType - BasicPlanCategory
            modelBuilder.Entity<BasicPlanCategory>()
                .HasOne(s => s.BasicPlanType)
                .WithMany(e => e.BasicPlanCategories)
                .HasForeignKey(s => s.BasicPlanTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlanType - BasicPlanLevel
            modelBuilder.Entity<BasicPlanLevel>()
                .HasOne(s => s.BasicPlanType)
                .WithMany(e => e.BasicPlanLevels)
                .HasForeignKey(s => s.BasicPlanTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlanType - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.BasicPlanType)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.BasicPlanTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlanCategory - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.BasicPlanCategory)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.BasicPlanCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlanLevel - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.BasicPlanLevel)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.PlanLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlanTargetAudience - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.PlanTargetAudience)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.TargetAudienceId)
                .OnDelete(DeleteBehavior.Restrict);

            // PackageDuration - ComboPlanDuration
            modelBuilder.Entity<ComboPlanDuration>()
                .HasOne(s => s.PackageDuration)
                .WithMany(e => e.ComboPlanDurations)
                .HasForeignKey(s => s.PackageDurationId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - Membership
            modelBuilder.Entity<Membership>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.Memberships)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - MembershipHistory
            modelBuilder.Entity<MembershipHistory>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.MembershipHistories)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - ComboPlanBasic
            modelBuilder.Entity<ComboPlanBasic>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.ComboPlanBasics)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - ComboPlanDuration
            modelBuilder.Entity<ComboPlanDuration>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.ComboPlanDurations)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // RoomInstance - BasicPlanRoom
            modelBuilder.Entity<BasicPlanRoom>()
                .HasOne(s => s.RoomInstance)
                .WithMany() // Nếu RoomInstance không có navigation reverse
                .HasForeignKey(s => s.RoomInstanceId) // sửa đúng tên FK bạn đang dùng
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - BasicPlanRoom
            modelBuilder.Entity<BasicPlanRoom>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.BasicPlanRooms)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - BasicPlanEntitlement
            modelBuilder.Entity<BasicPlanEntitlement>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.BasicPlanEntitlements)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComboPlan - Membership
            modelBuilder.Entity<Membership>()
                .HasOne(s => s.ComboPlan)
                .WithMany(e => e.Memberships)
                .HasForeignKey(s => s.ComboPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComboPlan - Membership
            modelBuilder.Entity<MembershipHistory>()
                .HasOne(s => s.ComboPlan)
                .WithMany(e => e.MembershipHistories)
                .HasForeignKey(s => s.ComboPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComboPlan - ComboPlanBasic
            modelBuilder.Entity<ComboPlanBasic>()
                .HasOne(s => s.ComboPlan)
                .WithMany(e => e.ComboPlanBasics)
                .HasForeignKey(s => s.ComboPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComboPlan - ComboPlanDuration
            modelBuilder.Entity<ComboPlanDuration>()
                .HasOne(s => s.ComboPlan)
                .WithMany(e => e.ComboPlanDurations)
                .HasForeignKey(s => s.ComboPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // PackageLevel - ComboPlan
            modelBuilder.Entity<ComboPlan>()
                .HasOne(s => s.PackageLevel)
                .WithMany(e => e.ComboPlans)
                .HasForeignKey(s => s.PackageLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - Media
            modelBuilder.Entity<Media>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.MediaGallery)
                .HasForeignKey(s => s.NextUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // AccommodationOption - Media
            modelBuilder.Entity<Media>()
                .HasOne(s => s.AccommodationOption)
                .WithMany(e => e.MediaGallery)
                .HasForeignKey(s => s.AccommodationOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - AccommodationOption
            modelBuilder.Entity<AccommodationOption>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.AccommodationOptions)
                .HasForeignKey(s => s.NextUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // RoomType - AccommodationOption
            modelBuilder.Entity<AccommodationOption>()
                .HasOne(s => s.RoomType)
                .WithMany(e => e.AccommodationOptions)
                .HasForeignKey(s => s.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // AccommodationOption - RoomInstance
            modelBuilder.Entity<RoomInstance>()
                .HasOne(s => s.AccommodationOption)
                .WithMany(e => e.Rooms)
                .HasForeignKey(s => s.AccommodationOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - EntitlementRule
            modelBuilder.Entity<EntitlementRule>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.EntitlementRules)
                .HasForeignKey(s => s.NextUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccommodationOption>()
                .Property(x => x.PricePerNight)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<BasicPlanRoom>()
                .Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,4)");


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>().HasData(
               new Location
               {
                   Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                   Name = "Hà Nội",
                   Description = "Khu vực miền Bắc",
                   Code = "HN"
               },
               new Location
               {
                   Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                   Name = "Đà Nẵng",
                   Description = "Khu vực miền Trung",
                   Code = "DN" 
               },
               new Location
               {
                   Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                   Name = "Hải Phòng",
                   Description = "Hải Phòng",
                   Code = "HP"
               }
           );


        }


    }
}
