using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Interfaces;
using UserService.API.DTOs.Responses;

namespace UserService.API.Controllers.User
{
    [ApiController]
    [Route("api/user/feedbacks")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitFeedback([FromBody] CreateFeedbackDto dto)
        {
            var accountId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (accountId == null)
                return Unauthorized("Missing user identity.");

            var result = await _feedbackService.CreateFeedbackAsync(Guid.Parse(accountId), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
