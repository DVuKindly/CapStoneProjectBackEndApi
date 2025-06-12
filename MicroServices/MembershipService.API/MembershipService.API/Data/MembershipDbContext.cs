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
        public DbSet<Media> Media { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<PackageDuration> PackageDurations { get; set; }
        public DbSet<BasicPackage> BasicPackages { get; set; }
        public DbSet<ComboPackage> ComboPackages { get; set; }
        public DbSet<ComboPackageService> ComboPackageServices { get; set; }
        public DbSet<ServicePricing> ServicePricings { get; set; }
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

            // PackageType - BasicPackage
            modelBuilder.Entity<BasicPackage>()
                .HasOne(s => s.PackageType)
                .WithMany(e => e.BasicPackages)
                .HasForeignKey(s => s.PackageTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // PackageDuration - BasicPackage
            modelBuilder.Entity<BasicPackage>()
                .HasOne(s => s.PackageDuration)
                .WithMany(e => e.BasicPackages)
                .HasForeignKey(s => s.PackageDurationId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPackage - ComboPackage
            modelBuilder.Entity<ComboPackage>()
                .HasOne(s => s.BasicPackage)
                .WithMany(e => e.ComboPackages)
                .HasForeignKey(s => s.BasicPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            
            // ComboPackage - ComboPackageService
            modelBuilder.Entity<ComboPackageService>()
                .HasOne(s => s.ComboPackage)
                .WithMany(e => e.ComboPackageServices)
                .HasForeignKey(s => s.ComboPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - ComboPackageService
            modelBuilder.Entity<ComboPackageService>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.ComboPackageServices)
                .HasForeignKey(s => s.NextUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - ServicePricing
            modelBuilder.Entity<ServicePricing>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.ServicePricings)
                .HasForeignKey(s => s.NextUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // PackageLevel - BasicPackage
            modelBuilder.Entity<BasicPackage>()
                .HasOne(s => s.PackageLevel)
                .WithMany(e => e.BasicPackages)
                .HasForeignKey(s => s.PackageLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPackage - ServicePricing
            modelBuilder.Entity<ServicePricing>()
                .HasOne(s => s.BasicPackage)
                .WithMany(e => e.ServicePricings)
                .HasForeignKey(s => s.BasicPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComboPackage - ServicePricing
            modelBuilder.Entity<ServicePricing>()
                .HasOne(s => s.ComboPackage)
                .WithMany(e => e.ServicePricings)
                .HasForeignKey(s => s.ComboPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - Media
            modelBuilder.Entity<Media>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.MediaGallery)
                .HasForeignKey(s => s.NexUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPackage - Media
            modelBuilder.Entity<Media>()
                .HasOne(s => s.BasicPackage)
                .WithMany(e => e.MediaGallery)
                .HasForeignKey(s => s.BasicPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // BasicPackage - BasicPackageService
            modelBuilder.Entity<BasicPackageService>()
                .HasOne(s => s.BasicPackage)
                .WithMany(e => e.BasicPackageServices)
                .HasForeignKey(s => s.BasicPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // NextUService - BasicPackageService
            modelBuilder.Entity<BasicPackageService>()
                .HasOne(s => s.NextUService)
                .WithMany(e => e.BasicPackageServices)
                .HasForeignKey(s => s.NextUServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

        }


    }
}
