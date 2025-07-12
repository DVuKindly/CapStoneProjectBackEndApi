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
                    Interests = request.
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
             Interests = user.Interests,
             CvUrl = user.CvUrl,
             Introduction = user.Introduction,
             PersonalityTraits = user.PersonalityTraits,

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







        public async Task<List<UserProfileShortDto>> GetProfilesByAccountIdsAsync(List<Guid> accountIds)
        {
            var profiles = await _db.UserProfiles
                .Where(p => accountIds.Contains(p.AccountId))
                .Include(p => p.LocationRegion)
                .Select(p => new UserProfileShortDto
                {
                    AccountId = p.AccountId,
                    LocationId = p.LocationId,
                    LocationName = p.LocationRegion != null ? p.LocationRegion.Name : null,
                    RoleType = p.RoleType
                })
                .ToListAsync();

            return profiles;
        }






        public async Task<BaseResponse> UpdateStatusAsync(Guid accountId, UpdateUserProfileStatusDto dto)
        {
            try
            {
                var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);

                if (user == null)
                    return new BaseResponse { Success = false, Message = "Không tìm thấy hồ sơ người dùng." };

             
                user.OnboardingStatus = "ApprovedMember";

              
                if (user.RoleType?.ToLower() != "member")
                {
                    user.RoleType = "member";
                }

                user.UpdatedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return new BaseResponse { Success = true, Message = "Cập nhật trạng thái hồ sơ thành công." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = $"Lỗi khi cập nhật trạng thái hồ sơ: {ex.Message}" };
            }
        }

        public async Task<List<UserProfileShortDto>> GetProfilesByRoleKeysAsync(string[] roleKeys)
        {
            var profiles = await _db.UserProfiles
                .Include(p => p.LocationRegion)
                .Where(p => roleKeys.Contains(p.RoleType)) 
                .ToListAsync();

            return profiles.Select(p => new UserProfileShortDto
            {
                AccountId = p.AccountId,
                RoleType = p.RoleType,
                LocationId = p.LocationId,
                LocationName = p.LocationRegion?.Name ?? ""
            }).ToList();
        }

        public async Task<UserProfileShortDto?> GetProfileShortDtoAsync(Guid accountId)
        {
            return await _db.UserProfiles
                .Where(x => x.AccountId == accountId)
                .Select(x => new UserProfileShortDto
                {
                    AccountId = x.AccountId,
              
                    LocationId = x.LocationId,
                    LocationName = x.LocationRegion.Name,
                    RoleType = x.RoleType
                })
                .FirstOrDefaultAsync();
        }


    }
}
