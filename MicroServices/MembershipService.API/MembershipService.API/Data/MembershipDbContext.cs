using MembershipService.API.Entities;
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
        public DbSet<Media> MediaGallery { get; set; }
        public DbSet<PackageDuration> PackageDurations { get; set; }
        public DbSet<BasicPlan> BasicPlans { get; set; }
        public DbSet<BasicPlanService> BasicPlanServices { get; set; }
        public DbSet<ComboPlan> ComboPlans { get; set; }
        public DbSet<ComboPlanService> ComboPlanServices { get; set; }
        //public DbSet<ServicePricing> ServicePricings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PackageLevel> PackageLevels { get; set; }
        
        //public DbSet<Membership> Memberships { get; set; }


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

            // PackageDuration - BasicPlan
            modelBuilder.Entity<BasicPlan>()
                .HasOne(s => s.PackageDuration)
                .WithMany(e => e.BasicPlans)
                .HasForeignKey(s => s.PackageDurationId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - ComboPlan
            modelBuilder.Entity<ComboPlan>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.ComboPlans)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComboPlan - ComboPlanService
            modelBuilder.Entity<ComboPlanService>()
                .HasOne(s => s.ComboPlan)
                .WithMany(e => e.ComboPlanServices)
                .HasForeignKey(s => s.ComboPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - ComboPlanService
            modelBuilder.Entity<ComboPlanService>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.ComboPlanServices)
                .HasForeignKey(s => s.NextUServiceId)
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
                .HasForeignKey(s => s.NexUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - Media
            modelBuilder.Entity<Media>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.MediaGallery)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPlan - BasicPlanService
            modelBuilder.Entity<BasicPlanService>()
                .HasOne(s => s.BasicPlan)
                .WithMany(e => e.BasicPlanServices)
                .HasForeignKey(s => s.BasicPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - BasicPlanService
            modelBuilder.Entity<BasicPlanService>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.BasicPlanServices)
                .HasForeignKey(s => s.NextUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

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
