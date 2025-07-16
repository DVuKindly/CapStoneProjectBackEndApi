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
        public DbSet<Interest> Interests { get; set; }
        public DbSet<PersonalityTrait> PersonalityTraits { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<UserPersonalityTrait> UserPersonalityTraits { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }


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


            modelBuilder.Entity<Membership>()
    .HasOne(m => m.UserProfile)
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
            // USER <--> INTEREST
            modelBuilder.Entity<UserInterest>()
                .HasKey(ui => new { ui.UserProfileId, ui.InterestId });

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.UserProfile)
                .WithMany(u => u.UserInterests)
                .HasForeignKey(ui => ui.UserProfileId);

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.Interest)
                .WithMany()
                .HasForeignKey(ui => ui.InterestId);

            // USER <--> PERSONALITY TRAIT
            modelBuilder.Entity<UserPersonalityTrait>()
                .HasKey(up => new { up.UserProfileId, up.PersonalityTraitId });

            modelBuilder.Entity<UserPersonalityTrait>()
                .HasOne(up => up.UserProfile)
                .WithMany(u => u.UserPersonalityTraits)
                .HasForeignKey(up => up.UserProfileId);

            modelBuilder.Entity<UserPersonalityTrait>()
                .HasOne(up => up.PersonalityTrait)
                .WithMany()
                .HasForeignKey(up => up.PersonalityTraitId);

            // USER <--> SKILL
            modelBuilder.Entity<UserSkill>()
                .HasKey(us => new { us.UserProfileId, us.SkillId });

            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.UserProfile)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(us => us.UserProfileId);

            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.Skill)
                .WithMany()
                .HasForeignKey(us => us.SkillId);


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
            modelBuilder.Entity<Interest>().HasData(
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Name = "Adventure travel" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Alternative energy" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Name = "Alternative medicine" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Name = "Animal welfare" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000005"), Name = "Astronomy" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000006"), Name = "Athletics" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000007"), Name = "Backpacking" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000008"), Name = "Badminton" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000009"), Name = "Baseball" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000010"), Name = "Basketball" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000011"), Name = "Beer tasting" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000012"), Name = "Bicycling" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000013"), Name = "Board games" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000014"), Name = "Bowling" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000015"), Name = "Brunch" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000016"), Name = "Camping" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000017"), Name = "Clubbing" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000018"), Name = "Comedy" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000019"), Name = "Conservation" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000020"), Name = "Cooking" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000021"), Name = "Crafts" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000022"), Name = "DIY – Do it Yourself" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000023"), Name = "Dancing" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000024"), Name = "Dining out" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000025"), Name = "Diving" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000026"), Name = "Drinking" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000027"), Name = "Education technology" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000028"), Name = "Entrepreneurship" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000029"), Name = "Environmental awareness" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000030"), Name = "Fencing" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000031"), Name = "Film" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000032"), Name = "Finance technology" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000033"), Name = "Fishing" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000034"), Name = "Fitness" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000035"), Name = "Frisbee" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000036"), Name = "Gaming" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000037"), Name = "Golf" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000038"), Name = "Happy hour" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000039"), Name = "Healing" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000040"), Name = "Hiking" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000041"), Name = "History" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000042"), Name = "Holistic health" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000043"), Name = "Horse riding" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000044"), Name = "Human rights" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000045"), Name = "Hunting" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000046"), Name = "Ice skating" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000047"), Name = "Innovation" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000048"), Name = "International travel" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000049"), Name = "Internet startups" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000050"), Name = "Investing" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000051"), Name = "Karaoke" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000052"), Name = "Kayaking" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000053"), Name = "Languages" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000054"), Name = "Literature" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000055"), Name = "Local culture" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000056"), Name = "Marketing" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000057"), Name = "Martial arts" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000058"), Name = "Meditation" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000059"), Name = "Mountain biking" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000060"), Name = "Music" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000061"), Name = "Natural parks" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000062"), Name = "Networking" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000063"), Name = "Neuroscience" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000064"), Name = "Nightlife" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000065"), Name = "Nutrition" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000066"), Name = "Outdoor adventure" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000067"), Name = "Outdoor sports" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000068"), Name = "Painting" },
         new Interest { Id = Guid.Parse("20000000-0000-0000-0000-000000000069"), Name = "Photography" }
     // ... và tiếp tục thêm các mục còn lại nếu bạn cần.
     );


            modelBuilder.Entity<PersonalityTrait>().HasData(
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000001"), Name = "Introvert" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000002"), Name = "Optimistic" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000003"), Name = "Extrovert" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000004"), Name = "Realistic" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000005"), Name = "Ambitious" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000006"), Name = "Easygoing" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000007"), Name = "Thoughtful" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000008"), Name = "Energetic" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000009"), Name = "Creative" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000010"), Name = "Reliable" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000011"), Name = "Adventurous" },
       new PersonalityTrait { Id = Guid.Parse("30000000-0000-0000-0000-000000000012"), Name = "Compassionate" }
   );


            modelBuilder.Entity<Skill>().HasData(
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000001"), Name = "A/B testing" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000002"), Name = "AI" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000003"), Name = "API development" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000004"), Name = "Accounting" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000005"), Name = "Administrative support" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000006"), Name = "Advertising" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000007"), Name = "Affiliate marketing" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000008"), Name = "Android development" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000009"), Name = "Animators" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000010"), Name = "Audio production" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000011"), Name = "Back-end development" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000012"), Name = "Blogging" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000013"), Name = "Bookkeeping" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000014"), Name = "Brand strategy" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000015"), Name = "Branding" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000016"), Name = "Business development" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000017"), Name = "CRM management" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000018"), Name = "Communication" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000019"), Name = "Community management" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000020"), Name = "Content" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000021"), Name = "Content marketing" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000022"), Name = "Copyediting" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000023"), Name = "Copywriting" },
             new Skill { Id = Guid.Parse("40000000-0000-0000-0000-000000000024"), Name = "Creative writing" }
       
         );



        }
    }
}
