using MembershipService.API.Dtos.Request;
using MembershipService.API.Enums;
using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] MediaUploadRequestDto request)
        {
            var result = await _mediaService.UploadAsync(request);
            return Ok(result);
        }

        [HttpGet("by-actor")]
        public async Task<IActionResult> GetByActor([FromQuery] ActorType actorType, [FromQuery] Guid actorId)
        {
            var result = await _mediaService.GetByActorAsync(actorType, actorId);
            return Ok(result);
        }
    }
}