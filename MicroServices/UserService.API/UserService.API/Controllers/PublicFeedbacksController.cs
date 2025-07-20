using Microsoft.AspNetCore.Mvc;
using UserService.API.Services.Interfaces;

namespace UserService.API.Controllers.Public
{
    [ApiController]
    [Route("api/user/publicfeedbacks")]
    public class PublicFeedbacksController : ControllerBase
    {
        private readonly IFeedbackQueryService _queryService;

        public PublicFeedbacksController(IFeedbackQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet("package")]
        public async Task<IActionResult> GetByPackage([FromQuery] Guid packageId)
        {
            var result = await _queryService.GetFeedbacksByPackageAsync(packageId);
            return Ok(result);
        }

        [HttpGet("service")]
        public async Task<IActionResult> GetByService([FromQuery] string serviceType, [FromQuery] Guid targetId)
        {
            var result = await _queryService.GetFeedbacksByServiceAsync(serviceType, targetId);
            return Ok(result);
        }
    }
}
