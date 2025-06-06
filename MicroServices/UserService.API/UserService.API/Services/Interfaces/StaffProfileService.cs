using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Implementations;

namespace UserService.API.Services.Interfaces
{
    public class StaffProfileService : IStaffProfileService
    {
        private readonly UserDbContext _db;

        public StaffProfileService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(CreateStaffProfileRequest request)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (user == null) return false;

            var staff = new StaffProfile
            {
                Id = request.AccountId,
                AccountId = request.AccountId,
                Phone = request.Phone,
                Department = request.Department,
                Note = request.Note,
                Level = request.Level,
                Address = request.Address,
                ManagerId = request.ManagerId,
               
                JoinedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _db.StaffProfiles.AddAsync(staff);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<StaffProfileResponse?> GetByAccountIdAsync(Guid accountId)
        {
            var staff = await _db.StaffProfiles.FirstOrDefaultAsync(s => s.AccountId == accountId);
            if (staff == null) return null;

            return new StaffProfileResponse
            {
                Id = staff.Id,
                AccountId = staff.AccountId,
                Phone = staff.Phone,
                Email = staff.Email,
                Department = staff.Department,
                Note = staff.Note,
                Level = staff.Level,
                Address = staff.Address,
                ManagerId = staff.ManagerId,
                JoinedDate = staff.JoinedDate,
                IsActive = staff.IsActive
            };
        }

        public async Task<bool> UpdateAsync(UpdateStaffProfileRequest request)
        {
            var staff = await _db.StaffProfiles.FirstOrDefaultAsync(s => s.AccountId == request.AccountId);
            if (staff == null) return false;

            staff.Phone = request.Phone;
            staff.Email = request.Email;
            staff.Department = request.Department;
            staff.Level = request.Level;
            staff.Address = request.Address;
            staff.Note = request.Note;
            staff.ManagerId = request.ManagerId;
            staff.IsActive = request.IsActive;

            _db.StaffProfiles.Update(staff);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<StaffProfileResponse>> GetAllAsync()
        {
            return await _db.StaffProfiles
                .Select(s => new StaffProfileResponse
                {
                    Id = s.Id,
                    AccountId = s.AccountId,
                    Phone = s.Phone,
                    Email = s.Email,
                    Department = s.Department,
                    Note = s.Note,
                    Level = s.Level,
                    Address = s.Address,
                    ManagerId = s.ManagerId,
                    JoinedDate = s.JoinedDate,
                    IsActive = s.IsActive
                }).ToListAsync();
        }
    }
}
