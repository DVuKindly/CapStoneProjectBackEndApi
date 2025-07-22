using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.API.DTOs.BasePositon;


namespace UserService.API.Services.Interfaces
{
    public interface IBasePositionService
    {
        // ===== CITY =====
        Task<BaseResponse<List<CityResponseDto>>> GetCitiesAsync();
        Task<BaseResponse<CityResponseDto>> CreateCityAsync(CreateCityBasePositionDTO dto);
        Task<BaseResponse> UpdateCityAsync(Guid id, UpdateCityDto dto);
        Task<BaseResponse> DeleteCityAsync(Guid id);

        // ===== LOCATION =====
        Task<BaseResponse<List<LocationResponseDto>>> GetLocationsAsync();
        Task<BaseResponse<LocationResponseDto>> CreateLocationAsync(CreateLocationBasePositionDTO dto);
        Task<BaseResponse> UpdateLocationAsync(Guid id, UpdateLocationDto dto);
        Task<BaseResponse> DeleteLocationAsync(Guid id);

        // ===== PROPERTY =====
        Task<BaseResponse<List<PropertyResponseDto>>> GetPropertiesAsync();
        Task<BaseResponse<PropertyResponseDto>> CreatePropertyAsync(CreatePropertyBasePositionDTO dto);
        Task<BaseResponse> UpdatePropertyAsync(Guid id, UpdatePropertyDto dto);
        Task<BaseResponse> DeletePropertyAsync(Guid id);
    }
}
