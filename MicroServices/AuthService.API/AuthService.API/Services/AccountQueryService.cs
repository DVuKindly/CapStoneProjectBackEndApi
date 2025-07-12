using AuthService.API.DTOs.Responses;
using AuthService.API.Repositories;
using AuthService.API.Services;

public class AccountQueryService : IAccountQueryService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserServiceClient _userServiceClient;

    public AccountQueryService(IUserRepository userRepository, IUserServiceClient userServiceClient)
    {
        _userRepository = userRepository;
        _userServiceClient = userServiceClient;
    }


    public async Task<List<AccountResponseDto>> GetAccountsByRoleKeysAsync(string[] roleKeys)
    {
        var users = await _userRepository.GetAccountsByRoleKeysAsync(roleKeys);
        var accountIds = users.Select(u => u.UserId).ToList();
        var profiles = await _userServiceClient.GetUserProfileShortDtosByIdsAsync(accountIds);

        return users.Select(user =>
        {
            var role = user.UserRoles.FirstOrDefault()?.Role;

            var profile = profiles.FirstOrDefault(p => p.AccountId == user.UserId);
            return new AccountResponseDto
            {
                AccountId = user.UserId,
                Username = user.UserName,
                Email = user.Email,
                Description = role?.Description,
                EmailVerified = user.EmailVerified,
                IsLocked = user.IsLocked,
                RefreshTokenExpiry = user.RefreshTokenExpiry,
                CreatedAt = user.CreatedAt,
                RoleKey = role?.RoleKey,
                RoleName = role?.RoleName,
                RoleType = profile?.RoleType,
                LocationId = profile?.LocationId,
                LocationName = profile?.LocationName,
              
            };
        }).ToList();
    }


    public async Task<List<AccountResponseDto>> GetAccountsByRoleKeysAndLocationAsync(Guid locationId, string[] roleKeys)
    {
        var users = await _userRepository.GetAccountsByRoleKeysAsync(roleKeys);
        var accountIds = users.Select(u => u.UserId).ToList();
        var profiles = await _userServiceClient.GetUserProfileShortDtosByIdsAsync(accountIds);

        return users
            .Select(user =>
            {
                var role = user.UserRoles.FirstOrDefault()?.Role;
                var profile = profiles.FirstOrDefault(p => p.AccountId == user.UserId && p.LocationId == locationId);

                if (profile == null) return null; // filter out users from other locations

                return new AccountResponseDto
                {
                    AccountId = user.UserId,
                    Username = user.UserName,
                    Email = user.Email,
                    Description = role?.Description,
                    EmailVerified = user.EmailVerified,
                    IsLocked = user.IsLocked,
                    RefreshTokenExpiry = user.RefreshTokenExpiry,
                    CreatedAt = user.CreatedAt,
                    RoleKey = role?.RoleKey,
                    RoleName = role?.RoleName,
                    RoleType = profile?.RoleType,
                    LocationId = profile?.LocationId,
                    LocationName = profile?.LocationName,
                
                };
            })
            .Where(dto => dto != null)
            .ToList();
    }


}
