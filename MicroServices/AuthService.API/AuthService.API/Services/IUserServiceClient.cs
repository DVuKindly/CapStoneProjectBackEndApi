﻿using AuthService.API.DTOs.AdminCreate;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace AuthService.API.Services
{
    public interface IUserServiceClient
    {
        Task CreateUserProfileAsync(Guid userId, string userName, string email, string roleType = "user", object? profileInfo = null);
        Task<List<LocationDto>> GetLocationsAsync();
        Task<bool> IsValidLocationAsync(Guid locationId);
        Task<string?> GetLocationNameAsync(Guid locationId);
        Task<bool> UpdateUserProfileStatusAsync(UserProfilePayload payload);
   
        Task<Dictionary<Guid, UserProfileDto>> GetProfilesByUserIdsAsync(IEnumerable<Guid> userIds);
        Task<List<UserProfileShortDto>> GetUserProfileShortDtosByIdsAsync(List<Guid> accountIds);
      
        Task<UserProfileShortDto?> GetUserProfileShortDtoByIdAsync(Guid accountId);
        Task<List<UserProfileShortDto>> GetUserProfilesByRoleKeysAsync(string[] roleKeys);
        Task<bool> IsValidCityAsync(Guid cityId);
        Task<bool> IsPropertyInCityAsync(Guid propertyId, Guid cityId);
        Task<string?> GetLocationDisplayNameAsync(Guid locationId);

    }
}


