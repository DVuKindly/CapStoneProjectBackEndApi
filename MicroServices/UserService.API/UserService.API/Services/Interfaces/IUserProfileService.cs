using System;
using System.Threading.Tasks;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<BaseResponse> CreateAsync(UserProfilePayload payload);
        Task<UserProfileDto> GetProfileAsync(Guid accountId);
        Task<UserProfileResponseDto> UpdateProfileAsync(Guid accountId, UpdateUserProfileDto dto);

        // Thêm method cập nhật trạng thái hồ sơ
        Task<BaseResponse> UpdateStatusAsync(Guid accountId, UpdateUserProfileStatusDto dto);
    }
}
