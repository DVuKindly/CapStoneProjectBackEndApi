using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class RoomViewOptionRepository : IRoomViewOptionRepository
    {
        private readonly MembershipDbContext _db;

        public RoomViewOptionRepository(MembershipDbContext db)
        {
            _db = db;
        }

        public async Task<List<RoomViewOption>> GetAllAsync()
        {
            return await _db.RoomViewOptions
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }

}
