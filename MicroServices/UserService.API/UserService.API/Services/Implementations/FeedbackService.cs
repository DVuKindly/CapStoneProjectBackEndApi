using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Interfaces;


namespace UserService.API.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly UserDbContext _db;

        public FeedbackService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<BaseResponse> CreateFeedbackAsync(Guid accountId, CreateFeedbackDto dto)
        {
            // Kiểm tra đã từng mua gói chưa
            var hasMembership = await _db.Memberships
                .AnyAsync(m => m.AccountId == accountId && m.PackageId == dto.PackageId);

            if (!hasMembership)
                return BaseResponse.Fail("Bạn chưa mua gói này, không thể đánh giá.");

            // Không cho đánh giá trùng
            var hasFeedback = await _db.Feedbacks
                .AnyAsync(f => f.AccountId == accountId && f.PackageId == dto.PackageId);

            if (hasFeedback)
                return BaseResponse.Fail("Bạn đã đánh giá gói này rồi.");

            // Tạo feedback
            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                PackageId = dto.PackageId,
                OverallRating = dto.OverallRating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow,
                Details = dto.Details?.Select(d => new FeedbackDetail
                {
                    Id = Guid.NewGuid(),
                    ServiceType = d.ServiceType,
                    ServiceTargetId = d.ServiceTargetId,
                    Rating = d.Rating,
                    Comment = d.Comment
                }).ToList() ?? new()
            };

            _db.Feedbacks.Add(feedback);
            await _db.SaveChangesAsync();

            return BaseResponse.Ok("Đánh giá thành công.");
        }
    }

}
