using Microsoft.EntityFrameworkCore;
using UserService.API.Data;

namespace UserService.API.Services.CleanData
{
    public class RejectedCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RejectedCleanupService> _logger;

        public RejectedCleanupService(IServiceScopeFactory scopeFactory, ILogger<RejectedCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();

                var cutoffTime = DateTime.UtcNow.AddHours(-24);
                var oldRejected = await db.PendingMembershipRequests
                    .Where(r => r.Status == "Rejected" && r.ApprovedAt <= cutoffTime)
                    .ToListAsync(stoppingToken);

                if (oldRejected.Any())
                {
                    db.PendingMembershipRequests.RemoveRange(oldRejected);
                    await db.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation($"Đã xóa {oldRejected.Count} yêu cầu bị từ chối quá 24h.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Kiểm tra mỗi giờ
            }
        }
    }

}
