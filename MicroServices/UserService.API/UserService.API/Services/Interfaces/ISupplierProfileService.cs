using System.Threading.Tasks;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface ISupplierProfileService
    {
        Task<BaseResponse> CreateAsync(UserProfilePayload request);
    }
}
