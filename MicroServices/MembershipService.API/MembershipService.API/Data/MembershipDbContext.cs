using System.Reflection.Emit;
using MembershipService.API.Entities;
using MembershipService.API.Enums;
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
        public DbSet<RoomSizeOption> RoomSizeOptions { get; set; }
        public DbSet<RoomViewOption> RoomViewOptions { get; set; }
        public DbSet<RoomFloorOption> RoomFloorOptions { get; set; }
        public DbSet<BedTypeOption> BedTypeOptions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EntitlementRule> EntitlementRules { get; set; }
        public DbSet<Media> MediaGallery { get; set; }
        public DbSet<PackageDuration> PackageDurations { get; set; }
        public DbSet<ComboPlanDuration> ComboPlanDurations { get; set; }
        public DbSet<BasicPlanType> BasicPlanTypes { get; set; }
        public DbSet<BasicPlan> BasicPlans { get; set; }
        public DbSet<PlanCategory> PlanCategories { get; set; }
        public DbSet<PlanLevel> PlanLevels { get; set; }
        public DbSet<PlanTargetAudience> PlanTargetAudiences { get; set; }
        public DbSet<BasicPlanEntitlement> BasicPlanEntitlements { get; set; }
        public DbSet<BasicPlanRoom> BasicPlanRooms { get; set; }
        public DbSet<ComboPlan> ComboPlans { get; set; }
        public DbSet<ComboPlanBasic> ComboPlanBasics { get; set; }
        public DbSet<Property> Propertys { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<City> Cities { get; set; }
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

            //City - Location
            modelBuilder.Entity<Location>()
                .HasOne(l => l.City)
                .WithMany()
                .HasForeignKey(l => l.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            //Location - Property
            modelBuilder.Entity<Property>()
.HasOne(p => p.Location)
                 .WithMany()
                 .HasForeignKey(p => p.LocationId)
                 .OnDelete(DeleteBehavior.Restrict);

            // Property - Service
            modelBuilder.Entity<NextUService>()
                .HasOne(s => s.Property)
                .WithMany(e => e.NextUServices)
                .HasForeignKey(s => s.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Property - AccommodationOption
            modelBuilder.Entity<AccommodationOption>()
                .HasOne(s => s.Property)
                .WithMany(e => e.AccommodationOptions)
                .HasForeignKey(s => s.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Property - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.Property)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Property - ComboPlan
            modelBuilder.Entity<ComboPlan>()
                .HasOne(s => s.Property)
                .WithMany(e => e.ComboPlans)
                .HasForeignKey(s => s.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlanType - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.BasicPlanType)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.BasicPlanTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlanCategory - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.BasicPlanCategory)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.BasicPlanCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlanLevel - BasicPlan
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

            // AccommodationOption - BasicPlanRoom
            modelBuilder.Entity<BasicPlanRoom>()
                .HasOne(s => s.AccommodationOption)
                .WithMany(a => a.BasicPlanRooms)
                .HasForeignKey(s => s.AccommodationOptionId)
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

            // PlanCategory - ComboPlan
            modelBuilder.Entity<ComboPlan>()
                .HasOne(s => s.PlanCategory)
                .WithMany(e => e.ComboPlans)
                .HasForeignKey(s => s.BasicPlanCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlanLevel - ComboPlan
            modelBuilder.Entity<ComboPlan>()
                .HasOne(s => s.PlanLevel)
                .WithMany(e => e.ComboPlans)
                .HasForeignKey(s => s.PlanLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlanTargetAudience - ComboPlan
            modelBuilder.Entity<ComboPlan>()
                .HasOne(s => s.PlanTargetAudience)
                .WithMany(e => e.ComboPlans)
                .HasForeignKey(s => s.TargetAudienceId)
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

            //RoomSizeOption - RoomInstance
            modelBuilder.Entity<RoomInstance>()
                .HasOne(r => r.RoomSizeOption)
                .WithMany(s => s.Rooms)
                .HasForeignKey(r => r.RoomSizeOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            //RoomViewOption - RoomInstance 
            modelBuilder.Entity<RoomInstance>()
                .HasOne(r => r.RoomViewOption)
                .WithMany(v => v.Rooms)
                .HasForeignKey(r => r.RoomViewOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            //RoomFloorOption - RoomInstance
            modelBuilder.Entity<RoomInstance>()
                .HasOne(r => r.RoomFloorOption)
                .WithMany(f => f.Rooms)
                .HasForeignKey(r => r.RoomFloorOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            //BedTypeOption-  RoomInstance
            modelBuilder.Entity<RoomInstance>()
                .HasOne(r => r.BedTypeOption)
                .WithMany(b => b.Rooms)
                .HasForeignKey(r => r.BedTypeOptionId)
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


            var cityHanoi = new City
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Name = "Hà Nội",
                Description = "Thủ đô Việt Nam",
                CreatedAt = new DateTime(2024, 1, 1)
            };

            var locationHoangCau = new Location
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                Name = "Hoàng Cầu",
                CityId = cityHanoi.Id,
                CreatedAt = new DateTime(2024, 1, 1)
            };

            modelBuilder.Entity<City>().HasData(cityHanoi);
            modelBuilder.Entity<Location>().HasData(locationHoangCau);

            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                    Name = "Hoàng Cầu Cơ sở 1",
                    Description = "Khu vực trụ sở chính Hoàng Cầu 1",
                    LocationId = locationHoangCau.Id,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Property
                {
                    Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                    Name = "Hoàng Cầu Cơ sở 2",
                    Description = "Khu vực trụ sở chính Hoàng Cầu 2",
                    LocationId = locationHoangCau.Id,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Property
                {
                    Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                    Name = "Hoàng Cầu Cơ sở 3",
                    Description = "Khu vực trụ sở chính Hoàng Cầu 3",
                    LocationId = locationHoangCau.Id,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            modelBuilder.Entity<RoomSizeOption>().HasData(
                new RoomSizeOption { Id = 1, Name = "Small" },
                new RoomSizeOption { Id = 2, Name = "Medium" },
                new RoomSizeOption { Id = 3, Name = "Large" }
);

            modelBuilder.Entity<RoomViewOption>().HasData(
                new RoomViewOption { Id = 1, Name = "Garden" },
                new RoomViewOption { Id = 2, Name = "Sea" },
                new RoomViewOption { Id = 3, Name = "Balcony" }
            );

            modelBuilder.Entity<RoomFloorOption>().HasData(
new RoomFloorOption { Id = 1, Name = "Low" },
                new RoomFloorOption { Id = 2, Name = "Mid" },
                new RoomFloorOption { Id = 3, Name = "High" },
                new RoomFloorOption { Id = 4, Name = "Terrace" }
            );

            modelBuilder.Entity<BedTypeOption>().HasData(
                new BedTypeOption { Id = 1, Name = "Single" },
                new BedTypeOption { Id = 2, Name = "Double" },
                new BedTypeOption { Id = 3, Name = "Queen" },
                new BedTypeOption { Id = 4, Name = "King" }
            );

            modelBuilder.Entity<PackageDuration>().HasData(
                new PackageDuration
                {
                    Id = 1,
                    Value = 1,
                    Unit = DurationUnit.Month,
                    Description = "1 tháng",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new PackageDuration
                {
                    Id = 2,
                    Value = 3,
                    Unit = DurationUnit.Month,
                    Description = "3 tháng",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new PackageDuration
                {
                    Id = 3,
                    Value = 6,
                    Unit = DurationUnit.Month,
                    Description = "6 tháng",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            modelBuilder.Entity<BasicPlanType>().HasData(
                new BasicPlanType
                {
                    Id = Guid.Parse("60000000-0000-0000-0000-000000000001"),
                    Code = "ACCOMMODATION",
                    Name = "Accommodation",
                    Description = "Các gói dịch vụ liên quan đến chỗ ở",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new BasicPlanType
                {
                    Id = Guid.Parse("60000000-0000-0000-0000-000000000003"),
                    Code = "LIFE_ACTIVITY",
                    Name = "Life Activity",
                    Description = "Các hoạt động trải nghiệm, xã hội, giải trí",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            modelBuilder.Entity<PlanCategory>().HasData(
                new PlanCategory
                {
                    Id = 1,
                    Name = "Dài hạn",
                    Description = "Áp dụng cho các gói từ 1 tháng trở lên",
                },
                new PlanCategory
                {
                    Id = 2,
                    Name = "Ngắn hạn",
                    Description = "Dành cho nhu cầu ngắn ngày hoặc linh hoạt",
                },
                new PlanCategory
                {
                    Id = 3,
                    Name = "Theo sự kiện",
                    Description = "Dành riêng cho các gói hoạt động theo sự kiện cụ thể",
                }
            );

            modelBuilder.Entity<PlanLevel>().HasData(
                new PlanLevel
                {
                    Id = 1,
                    Name = "Cơ bản",
                },
                new PlanLevel
                {
                    Id = 2,
                    Name = "Tiêu chuẩn",
                },
                new PlanLevel
                {
                    Id = 3,
                    Name = "Cao cấp",
                }
            );

            modelBuilder.Entity<PlanTargetAudience>().HasData(
                new PlanTargetAudience
                {
                    Id = 1,
                    Name = "Cá nhân"
                },
                new PlanTargetAudience
                {
                    Id = 2,
                    Name = "Nhóm"
                },
                new PlanTargetAudience
                {
                    Id = 3,
                    Name = "Doanh nghiệp"
                }
            );



            base.OnModelCreating(modelBuilder);
        }
    }
}