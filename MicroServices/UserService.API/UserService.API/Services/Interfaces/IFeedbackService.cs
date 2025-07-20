using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IFeedbackService
    {
     
        Task<BaseResponse> CreateFeedbackAsync(Guid accountId, CreateFeedbackDto dto);
       
    }
}
