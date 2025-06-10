using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<BaseResponse> CreateAsync(UserProfilePayload request)
        {
            try
            {
                if (await _db.UserProfiles.AnyAsync(x => x.AccountId == request.AccountId))
                    return new BaseResponse { Success = false, Message = "Tài khoản đã có profile." };

                var entity = new UserProfile
                {
                    Id = request.AccountId,
                    AccountId = request.AccountId,
                    FullName = request.FullName,
                    RoleType = request.RoleType,
                    LocationId = request.LocationId == Guid.Empty ? null : request.LocationId,
                    IsCompleted = false,

                    Phone = request.Phone,
                    Gender = request.Gender,
                    DOB = string.IsNullOrEmpty(request.DOB) ? null : DateTime.Parse(request.DOB),
                    Address = request.Address,
                    Note = request.Note,
                    OnboardingStatus = request.OnboardingStatus ?? "Pending",
                    CreatedByAdminId = request.CreatedByAdminId,
                    CreatedAt = DateTime.UtcNow
                };

                await _db.UserProfiles.AddAsync(entity);
                await _db.SaveChangesAsync();

                return new BaseResponse { Success = true, Message = "Tạo hồ sơ thành công." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = $"Lỗi khi lưu profile: {ex.Message}" };
            }
        }



        // ✅ GET profile cá nhân
        public async Task<UserProfileDto> GetProfileAsync(Guid accountId)
        {
            var user = await _db.UserProfiles
                .Include(u => u.LocationRegion)
                .FirstOrDefaultAsync(u => u.AccountId == accountId);

            if (user == null) return null!;

            return new UserProfileDto
            {
                AccountId = user.AccountId,
                FullName = user.FullName,
                Phone = user.Phone,
                Gender = user.Gender,
                DOB = user.DOB,
                AvatarUrl = user.AvatarUrl,
                SocialLinks = user.SocialLinks,
                LocationId = user.LocationId,
                LocationName = user.LocationRegion?.Name,
                Address = user.Address,
                OnboardingStatus = user.OnboardingStatus,
                Note = user.Note,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<UserProfileResponseDto> UpdateProfileAsync(Guid accountId, UpdateUserProfileDto dto)
        {
            var user = await _db.UserProfiles
                .Include(u => u.LocationRegion)
                .FirstOrDefaultAsync(u => u.AccountId == accountId);

            if (user == null)
            {
                return new UserProfileResponseDto
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng."
                };
            }

            // ✅ Cập nhật thông tin cho phép
            user.FullName = dto.FullName;
            user.Phone = dto.Phone;
            user.Gender = dto.Gender;
            user.DOB = dto.DOB;
            user.AvatarUrl = dto.AvatarUrl;
            user.SocialLinks = dto.SocialLinks;
            user.Address = dto.Address;
            user.Interests = dto.Interests;
            user.PersonalityTraits = dto.PersonalityTraits;
            user.Introduction = dto.Introduction;
            user.CvUrl = dto.CvUrl;
            user.Note = dto.Note;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            // ✅ Lấy lại thông tin khu vực (nếu cần)
            var locationRegion = await _db.LocationRegions
                .FirstOrDefaultAsync(r => r.Id == user.LocationId);

            return new UserProfileResponseDto
            {
                Success = true,
                Message = "Cập nhật hồ sơ thành công.",
                Data = new UserProfileDto
                {
                    AccountId = user.AccountId,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Gender = user.Gender,
                    DOB = user.DOB,
                    AvatarUrl = user.AvatarUrl,
                    SocialLinks = user.SocialLinks,
                    Address = user.Address,
                    LocationId = user.LocationId,
                    LocationName = locationRegion?.Name,
                    OnboardingStatus = user.OnboardingStatus,
                    Note = user.Note,
                    UpdatedAt = user.UpdatedAt,
                    Interests = user.Interests,
                    PersonalityTraits = user.PersonalityTraits,
                    Introduction = user.Introduction,
                    CvUrl = user.CvUrl
                }
            };
        }






    }
}
