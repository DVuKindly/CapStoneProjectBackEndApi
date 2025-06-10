using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class StaffOnboardingService : IStaffOnboardingService
    {
        private readonly UserDbContext _db;

        public StaffOnboardingService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffAccountId, ApproveMembershipRequestDto dto)
        {
            var staff = await _db.StaffProfiles.FirstOrDefaultAsync(s => s.AccountId == staffAccountId);
            if (staff == null || staff.LocationId == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Không xác định được khu vực của nhân viên duyệt."
                };
            }

            var request = await _db.PendingMembershipRequests
                .Include(r => r.UserProfile)
                .FirstOrDefaultAsync(r => r.Id == dto.RequestId);

            if (request == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu đăng ký."
                };
            }

            if (request.LocationId != staff.LocationId)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Bạn không có quyền duyệt yêu cầu ở khu vực này."
                };
            }

            // Nếu hợp lệ → duyệt
            request.Status = "PendingPayment"; // hoặc "Approved" nếu thanh toán xong
            request.ApprovedAt = DateTime.UtcNow;

            // Cập nhật trạng thái Onboarding của User
            if (request.UserProfile != null)
            {
                request.UserProfile.OnboardingStatus = "PendingPayment";
                request.UserProfile.LocationId = request.LocationId; // gán lại cho chắc
            }

            await _db.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Duyệt yêu cầu thành công. Chờ thanh toán từ người dùng."
            };
        }

    }
}
