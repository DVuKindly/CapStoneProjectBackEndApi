using Microsoft.EntityFrameworkCore;
using PaymentService.API.Entities;

namespace PaymentService.API.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure PaymentRequest
            modelBuilder.Entity<PaymentRequest>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasIndex(p => p.RequestCode)
                      .IsUnique();

                entity.Property(p => p.PaymentMethod)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(p => p.Status)
                      .HasMaxLength(30)
                      .IsRequired();

                entity.Property(p => p.ReturnUrl)
                      .HasMaxLength(500);

                entity.Property(p => p.ExtraData)
                      .HasMaxLength(1000);

                entity.Property(p => p.FailureReason)
                      .HasMaxLength(1000);

                entity.Property(p => p.FailureCode)
                      .HasMaxLength(100);

                entity.Property(p => p.RedisKey)
                      .HasMaxLength(200);
            });

            // Configure PaymentTransaction
            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.TransactionId)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(t => t.Gateway)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(t => t.GatewayResponse)
                      .HasColumnType("nvarchar(max)");

                entity.Property(t => t.Status)
                      .HasMaxLength(30);

                entity.HasOne(t => t.PaymentRequest)
                      .WithMany(p => p.Transactions)
                      .HasForeignKey(t => t.PaymentRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
