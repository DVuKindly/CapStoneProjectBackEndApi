using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class RoomFloorOptionRepository : IRoomFloorOptionRepository
    {
        private readonly MembershipDbContext _db;

        public RoomFloorOptionRepository(MembershipDbContext db)
        {
            _db = db;
        }

        public async Task<List<RoomFloorOption>> GetAllAsync()
        {
            return await _db.RoomFloorOptions
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }

}
