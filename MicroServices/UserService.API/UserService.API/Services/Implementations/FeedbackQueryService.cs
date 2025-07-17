using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class FeedbackQueryService : IFeedbackQueryService
    {
        private readonly UserDbContext _db;

        public FeedbackQueryService(UserDbContext db)
        {
            _db = db;
        }

        // Xem feedback theo gói
        public async Task<PublicFeedbackResponseDto> GetFeedbacksByPackageAsync(Guid packageId)
        {
            var feedbacks = await _db.Feedbacks
                .Include(f => f.User)
                .Where(f => f.PackageId == packageId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            if (!feedbacks.Any())
                return new PublicFeedbackResponseDto();

            var avg = Math.Round(feedbacks.Average(f => f.OverallRating), 1);
            var dist = feedbacks.GroupBy(f => f.OverallRating)
                                .ToDictionary(g => g.Key, g => g.Count());

            return new PublicFeedbackResponseDto
            {
                AverageRating = avg,
                TotalCount = feedbacks.Count,
                StarDistribution = Enumerable.Range(1, 5)
                    .ToDictionary(i => i, i => dist.ContainsKey(i) ? dist[i] : 0),
                Feedbacks = feedbacks.Select(f => new PublicFeedbackItemDto
                {
                    MaskedName = MaskName(f.User?.FullName),
                    AvatarUrl = f.User?.AvatarUrl ?? "",
                    Rating = f.OverallRating,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt
                }).ToList()
            };
        }

        // Xem feedback theo dịch vụ
        public async Task<PublicFeedbackResponseDto> GetFeedbacksByServiceAsync(string serviceType, Guid targetId)
        {
            var details = await _db.FeedbackDetails
                .Include(d => d.Feedback).ThenInclude(f => f.User)
                .Where(d => d.ServiceType == serviceType && d.ServiceTargetId == targetId)
                .OrderByDescending(d => d.Feedback.CreatedAt)
                .ToListAsync();

            if (!details.Any())
                return new PublicFeedbackResponseDto();

            var avg = Math.Round(details.Average(d => d.Rating), 1);
            var dist = details.GroupBy(d => d.Rating)
                              .ToDictionary(g => g.Key, g => g.Count());

            return new PublicFeedbackResponseDto
            {
                AverageRating = avg,
                TotalCount = details.Count,
                StarDistribution = Enumerable.Range(1, 5)
                    .ToDictionary(i => i, i => dist.ContainsKey(i) ? dist[i] : 0),
                Feedbacks = details.Select(d => new PublicFeedbackItemDto
                {
                    MaskedName = MaskName(d.Feedback?.User?.FullName),
                    AvatarUrl = d.Feedback?.User?.AvatarUrl ?? "",
                    Rating = d.Rating,
                    Comment = d.Comment,
                    CreatedAt = d.Feedback?.CreatedAt ?? DateTime.UtcNow
                }).ToList()
            };
        }

        private string MaskName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Ẩn danh";
            if (name.Length <= 3) return name.Substring(0, 1) + "***";
            return name.Substring(0, 3) + "***";
        }
    }


}
