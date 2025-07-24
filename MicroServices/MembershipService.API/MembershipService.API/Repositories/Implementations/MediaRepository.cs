using MembershipService.API.Data;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Enums;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace MembershipService.API.Repositories.Implementations
{
    public class MediaRepository : IMediaRepository
    {
        private readonly MembershipDbContext _context;

        public MediaRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<Guid, List<MediaResponseDto>>> GetGroupedByActorAsync(ActorType actorType, List<Guid> actorIds)
        {
            var medias = await _context.MediaGallery
                .Where(m => m.ActorType == actorType && actorIds.Contains(m.ActorId))
                .ToListAsync();

            return medias
                .GroupBy(m => m.ActorId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(m => new MediaResponseDto
                    {
                        Id = m.Id,
                        Url = m.Url,
                        Type = m.Type,
                        Description = m.Description,
                        ActorId = m.ActorId,
                        ActorType = m.ActorType
                    }).ToList()
                );
        }
    }
}