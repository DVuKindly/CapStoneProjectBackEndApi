using System.Threading.Tasks;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface ICoachProfileService
    {
        Task<BaseResponse> CreateAsync(UserProfilePayload payload);
    }
}
