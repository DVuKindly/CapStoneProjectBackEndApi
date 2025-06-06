using Microsoft.EntityFrameworkCore;
using UserService.API.Constants;
using UserService.API.Data;
using UserService.API.DTOs.Requests;

using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserDbContext _db;

        public UserProfileService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(CreateUserProfileRequest request)
        {
            if (await _db.UserProfiles.AnyAsync(u => u.AccountId == request.AccountId))
                return false; // Ngăn trùng accountId
            var user = new UserProfile
            {
                Id = request.AccountId,
                AccountId = request.AccountId,
                FullName = request.FullName,
                RoleType = request.RoleType ?? "User",
                Phone = request.Phone,
                Gender = request.Gender,
                DOB = !string.IsNullOrEmpty(request.DOB) && DateTime.TryParse(request.DOB, out var dob) ? dob : null,
                Location = request.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsCompleted = false,
                IsVerifiedByAdmin = false,
                OnboardingStatus = request.RoleType == "admin"
                    ? "AdminSystem"
                    : (!string.IsNullOrWhiteSpace(request.OnboardingStatus)
                        ? request.OnboardingStatus
                        : OnboardingStatuses.PendingPackageSelection)
            };

            await _db.UserProfiles.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateUserProfileRequest request)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (user == null) return false;

            user.FullName = request.FullName ?? user.FullName;
            user.Phone = request.Phone ?? user.Phone;
            user.Gender = request.Gender ?? user.Gender;
            user.Location = request.Location ?? user.Location;
            user.AvatarUrl = request.AvatarUrl ?? user.AvatarUrl;
            user.SocialLinks = request.SocialLinks ?? user.SocialLinks;
            user.RoleType = request.RoleType ?? user.RoleType;

            if (!string.IsNullOrEmpty(request.DOB) && DateTime.TryParse(request.DOB, out var parsedDob))
            {
                user.DOB = parsedDob;
            }

            // Những field nhạy cảm nên bị giới hạn quyền (chỉ admin/staff_onboarding mới set được)
            // Cho phép update nếu truyền lên từ luồng đặc biệt
            user.IsCompleted = request.IsCompleted ?? user.IsCompleted;
            user.IsVerifiedByAdmin = request.IsVerifiedByAdmin ?? user.IsVerifiedByAdmin;
            user.OnboardingStatus = request.OnboardingStatus ?? user.OnboardingStatus;

            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<UserProfileResponse?> GetByAccountIdAsync(Guid accountId)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
            if (user == null) return null;

            return new UserProfileResponse
            {
                AccountId = user.AccountId,
                FullName = user.FullName,
                Phone = user.Phone,
                Gender = user.Gender,
                DOB = user.DOB,
                Location = user.Location,
                RoleType = user.RoleType,
                OnboardingStatus = user.OnboardingStatus,
                IsCompleted = user.IsCompleted
            };
        }

        public async Task<UserProfileStatusResponse?> GetStatusAsync(Guid accountId)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
            if (user == null) return null;

            return new UserProfileStatusResponse
            {
                AccountId = user.AccountId,
                IsCompleted = user.IsCompleted,
                OnboardingStatus = user.OnboardingStatus
            };
        }

        public async Task<CheckCanPromoteResponse> CheckCanPromoteAsync(Guid accountId)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
            if (user == null)
            {
                return new CheckCanPromoteResponse { CanPromote = false, Reason = "User not found" };
            }

            if (!user.IsCompleted)
            {
                return new CheckCanPromoteResponse
                {
                    CanPromote = false,
                    Reason = "Hồ sơ chưa hoàn chỉnh"
                };
            }

            if (user.OnboardingStatus != OnboardingStatuses.ApprovedByStaff)
            {
                return new CheckCanPromoteResponse
                {
                    CanPromote = false,
                    Reason = $"Trạng thái chưa duyệt: {user.OnboardingStatus}"
                };
            }

            return new CheckCanPromoteResponse { CanPromote = true };
        }

        public async Task<List<UserProfileStatusResponse>> GetIncompleteProfilesAsync()
        {
            return await _db.UserProfiles
                .Where(u => !u.IsCompleted)
                .Select(u => new UserProfileStatusResponse
                {
                    AccountId = u.AccountId,
                    IsCompleted = u.IsCompleted,
                    OnboardingStatus = u.OnboardingStatus
                })
                .ToListAsync();
        }
    }
}
