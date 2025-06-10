using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class StaffProfileService : IStaffProfileService
    {
        private readonly UserDbContext _db;
        public StaffProfileService(UserDbContext db) => _db = db;

        public async Task<bool> CreateAsync(UserProfilePayload request)
        {
            var staff = new StaffProfile
            {
                Id = request.AccountId,
                Phone = request.Phone,
                AccountId = request.AccountId,
                Department = request.Department,
                LocationId = request.LocationId,
                ManagerId = request.ManagerId,
                Level = request.Level,
                Note = request.Note
            };

            await _db.StaffProfiles.AddAsync(staff);
            return await _db.SaveChangesAsync() > 0;
        }
    }

}
