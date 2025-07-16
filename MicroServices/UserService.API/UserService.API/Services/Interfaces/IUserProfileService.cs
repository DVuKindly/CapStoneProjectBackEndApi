using System;
using System.Threading.Tasks;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;

namespace UserService.API.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<BaseResponse> CreateAsync(UserProfilePayload payload);
        Task<UserProfileDto> GetProfileAsync(Guid accountId);
        Task<UserProfileResponseDto> UpdateProfileAsync(Guid accountId, UpdateUserProfileDto dto);

        // Thêm method cập nhật trạng thái hồ sơ
        Task<BaseResponse> UpdateStatusAsync(Guid accountId, UpdateUserProfileStatusDto dto);
        Task<List<UserProfileShortDto>> GetProfilesByAccountIdsAsync(List<Guid> accountIds);
        Task<List<UserProfileShortDto>> GetProfilesByRoleKeysAsync(string[] roleKeys);
        Task<UserProfileShortDto?> GetProfileShortDtoAsync(Guid accountId);

        Task<UserProfile?> GetCurrentUserProfileAsync(Guid accountId);


    }
}
