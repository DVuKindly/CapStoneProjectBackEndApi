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
    public class CoachProfileService : ICoachProfileService
    {
        private readonly UserDbContext _db;

        public CoachProfileService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<BaseResponse> CreateAsync(UserProfilePayload request)
        {
           

            var userProfile = new UserProfile
            {
                Id = request.AccountId,
                AccountId = request.AccountId,
                FullName = request.FullName,
                Phone = request.Phone,
                Gender = request.Gender,
              
                Address = request.Address,
                LocationId = request.LocationId,
                RoleType = request.RoleType,
                OnboardingStatus = request.OnboardingStatus,
                Note = request.Note,
                CreatedByAdminId = request.CreatedByAdminId,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
              
            };

            await _db.UserProfiles.AddAsync(userProfile);
            await _db.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Coach profile created successfully."
            };
        }
    }
}
