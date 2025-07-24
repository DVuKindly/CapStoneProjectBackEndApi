using MembershipService.API.Data;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Enums;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace MembershipService.API.Services.Implementations
{
    public class MediaService : IMediaService
    {
        private readonly MembershipDbContext _context;
        private readonly IBlobService _blobService;
        private readonly IMediaRepository _mediaRepository;

        public MediaService(MembershipDbContext context, IBlobService blobService, IMediaRepository mediaRepository)
        {
            _context = context;
            _blobService = blobService;
            _mediaRepository = mediaRepository;
        }

        public async Task<MediaResponseDto> UploadAsync(MediaUploadRequestDto request)
        {
            // Validate actor exists
            bool isValid = request.ActorType switch
            {
                ActorType.City => await _context.NextUServices.AnyAsync(x => x.Id == request.ActorId),
                ActorType.Location => await _context.Locations.AnyAsync(x => x.Id == request.ActorId),
                ActorType.Property => await _context.Propertys.AnyAsync(x => x.Id == request.ActorId),
                ActorType.Room => await _context.Rooms.AnyAsync(x => x.Id == request.ActorId),
                _ => false
            };

            if (!isValid)
                throw new Exception("Invalid ActorId for the given ActorType.");

            // Upload file
            var url = await _blobService.UploadAsync(request.File);

            var media = new Media
            {
                Id = Guid.NewGuid(),
                Url = url,
                Type = request.Type,
                Description = request.Description,
                ActorId = request.ActorId,
                ActorType = request.ActorType
            };

            _context.MediaGallery.Add(media);
            await _context.SaveChangesAsync();

            return new MediaResponseDto
            {
                Id = media.Id,
                Url = media.Url,
                Type = media.Type,
                Description = media.Description,
                ActorId = media.ActorId,
                ActorType = media.ActorType
            };
        }

        public async Task<List<MediaResponseDto>> GetByActorAsync(ActorType actorType, Guid actorId)
        {
            var dict = await _mediaRepository.GetGroupedByActorAsync(actorType, new List<Guid> { actorId });
            return dict.TryGetValue(actorId, out var list) ? list : new List<MediaResponseDto>();
        }

        public async Task<Dictionary<Guid, List<MediaResponseDto>>> GetGroupedByActorAsync(ActorType actorType, List<Guid> actorIds)
        {
            return await _mediaRepository.GetGroupedByActorAsync(actorType, actorIds);
        }
    }
}