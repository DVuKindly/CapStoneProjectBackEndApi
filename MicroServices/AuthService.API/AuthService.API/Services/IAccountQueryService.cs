using AuthService.API.DTOs.Responses;

namespace AuthService.API.Services
{
    public interface IAccountQueryService
    {
        Task<List<AccountResponseDto>> GetAccountsByRoleKeysAsync(string[] roleKeys);
        Task<List<AccountResponseDto>> GetAccountsByRoleKeysAndLocationAsync(Guid locationId, string[] roleKeys);

    }

}
