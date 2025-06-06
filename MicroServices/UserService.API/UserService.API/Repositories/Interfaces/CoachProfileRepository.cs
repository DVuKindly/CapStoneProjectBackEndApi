using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.Entities;
using UserService.API.Repositories.Implementations;

namespace UserService.API.Repositories.Interfaces
{
    public class CoachProfileRepository : ICoachProfileRepository
    {
        private readonly UserDbContext _db;

        public CoachProfileRepository(UserDbContext db) => _db = db;

        public Task<UserProfile?> GetUserByAccountIdAsync(Guid accountId) =>
            _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);

        public async Task AddCoachProfileAsync(CoachProfile profile)
        {
            await _db.CoachProfiles.AddAsync(profile);
            await _db.SaveChangesAsync();
        }

       
    }
}
