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

    }
}
