using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IFeedbackQueryService
    {
      
        Task<PublicFeedbackResponseDto> GetFeedbacksByPackageAsync(Guid packageId);

     
        Task<PublicFeedbackResponseDto> GetFeedbacksByServiceAsync(string serviceType, Guid targetId);
    }
}
