using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.BasePositon;
using UserService.API.DTOs.SyncPosition;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class BasePositionService : IBasePositionService
    {
        private readonly UserDbContext _db;
        private readonly IMembershipServiceClient _membershipServiceClient;
        public BasePositionService(UserDbContext db, IMembershipServiceClient membershipServiceClient)
        {
            _db = db;
            _membershipServiceClient = membershipServiceClient;
        }

        // ===== CITY =====
        public async Task<BaseResponse<List<CityResponseDto>>> GetCitiesAsync()
        {
            var cities = await _db.Cities
                .Select(c => new CityResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToListAsync();

            return BaseResponse<List<CityResponseDto>>.Ok(cities, "City list fetched successfully.");
        }

        public async Task<BaseResponse<CityResponseDto>> CreateCityAsync(CreateCityBasePositionDTO dto)
        {
            var entity = new City
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description
            };

            _db.Cities.Add(entity);
            await _db.SaveChangesAsync();
            await _membershipServiceClient.SyncCityAsync(new SyncCityDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            });

            var response = new CityResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

            return BaseResponse<CityResponseDto>.Ok(response, "City created successfully.");
        }

        public async Task<BaseResponse> UpdateCityAsync(Guid id, UpdateCityDto dto)
        {
            var city = await _db.Cities.FindAsync(id);
            if (city == null)
                return BaseResponse.Fail("City not found.");

            city.Name = dto.Name;
            city.Description = dto.Description;

            await _db.SaveChangesAsync();
            return BaseResponse.Ok("City updated successfully.");
        }

        public async Task<BaseResponse> DeleteCityAsync(Guid id)
        {
            var city = await _db.Cities.FindAsync(id);
            if (city == null)
                return BaseResponse.Fail("City not found.");

            var locations = await _db.Locations.Where(l => l.CityId == id).ToListAsync();
            if (locations.Any())
            {
                return BaseResponse.Fail($"Cannot delete city '{city.Name}' because it contains {locations.Count} location(s). Please delete those locations first.");
            }

            _db.Cities.Remove(city);
            await _db.SaveChangesAsync();

            await _membershipServiceClient.DeleteCityAsync(city.Id); // 👈 THÊM DÒNG NÀY

            return BaseResponse.Ok("City deleted successfully.");
        }



        // ===== LOCATION =====
        public async Task<BaseResponse<List<LocationResponseDto>>> GetLocationsAsync()
        {
            var locations = await _db.Locations
                .Include(l => l.City)
                .Select(l => new LocationResponseDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    Description = l.Description,
                    CityId = l.CityId,
                    CityName = l.City.Name
                }).ToListAsync();

            return BaseResponse<List<LocationResponseDto>>.Ok(locations, "Location list fetched successfully.");
        }

        public async Task<BaseResponse<LocationResponseDto>> CreateLocationAsync(CreateLocationBasePositionDTO dto)
        {
            var entity = new Location
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CityId = dto.CityId
            };

            _db.Locations.Add(entity);
            await _db.SaveChangesAsync();
            await _membershipServiceClient.SyncLocationAsync(new SyncLocationDto
            {
                Id = entity.Id,
                Name = entity.Name,
                //Description = entity.Description,
                CityId = entity.CityId
            });

            var city = await _db.Cities.FindAsync(dto.CityId);

            var response = new LocationResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CityId = city!.Id,
                CityName = city.Name
            };

            return BaseResponse<LocationResponseDto>.Ok(response, "Location created successfully.");
        }

        public async Task<BaseResponse> UpdateLocationAsync(Guid id, UpdateLocationDto dto)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location == null)
                return BaseResponse.Fail("Location not found.");

            location.Name = dto.Name;
            location.Description = dto.Description;
            location.CityId = dto.CityId;
            await _membershipServiceClient.SyncLocationAsync(new SyncLocationDto
            {
                Id = location.Id,
                Name = location.Name,
                //Description = location.Description,
                CityId = location.CityId
            });

            await _db.SaveChangesAsync();
            return BaseResponse.Ok("Location updated successfully.");
        }

        public async Task<BaseResponse> DeleteLocationAsync(Guid id)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location == null)
                return BaseResponse.Fail("Location not found.");

            var properties = await _db.Propertys.Where(p => p.LocationId == id).ToListAsync();
            if (properties.Any())
            {
                return BaseResponse.Fail($"Cannot delete location '{location.Name}' because it contains {properties.Count} property(ies). Please delete those properties first.");
            }

            _db.Locations.Remove(location);
            await _db.SaveChangesAsync();

            await _membershipServiceClient.DeleteLocationAsync(location.Id); // 👈 THÊM DÒNG NÀY

            return BaseResponse.Ok("Location deleted successfully.");
        }



        public async Task<BaseResponse<List<PropertyResponseDto>>> GetPropertiesAsync()
        {
            var properties = await _db.Propertys
                .Include(p => p.Location)
                    .ThenInclude(l => l.City)
                .ToListAsync(); // lấy xong rồi mới xử lý null

            var responseList = properties.Select(p => new PropertyResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                LocationId = p.LocationId ?? Guid.Empty,
                LocationName = p.Location != null ? p.Location.Name : "Unknown",
                CityId = p.Location?.CityId ?? Guid.Empty,
                CityName = p.Location?.City != null ? p.Location.City.Name : "Unknown"
            }).ToList();

            return BaseResponse<List<PropertyResponseDto>>.Ok(responseList, "Property list fetched successfully.");
        }


        public async Task<BaseResponse<PropertyResponseDto>> CreatePropertyAsync(CreatePropertyBasePositionDTO dto)
        {
            var entity = new Property
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                LocationId = dto.LocationId
            };

            _db.Propertys.Add(entity);
            await _db.SaveChangesAsync();
            await _membershipServiceClient.SyncPropertyAsync(new SyncPropertyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                LocationId = entity.LocationId
            });

            var location = await _db.Locations.Include(l => l.City).FirstAsync(l => l.Id == dto.LocationId);

            var response = new PropertyResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                LocationId = location.Id,
                LocationName = location.Name,
                CityId = location.City.Id,
                CityName = location.City.Name
            };

            return BaseResponse<PropertyResponseDto>.Ok(response, "Property created successfully.");
        }

        public async Task<BaseResponse> UpdatePropertyAsync(Guid id, UpdatePropertyDto dto)
        {
            var property = await _db.Propertys.FindAsync(id);
            if (property == null)
                return BaseResponse.Fail("Property not found.");

            property.Name = dto.Name;
            property.Description = dto.Description;
            property.LocationId = dto.LocationId;

            await _db.SaveChangesAsync();
            await _membershipServiceClient.SyncPropertyAsync(new SyncPropertyDto
            {
                Id = property.Id,
                Name = property.Name,
                Description = property.Description,
                LocationId = property.LocationId
            });

            return BaseResponse.Ok("Property updated successfully.");
        }

        public async Task<BaseResponse> DeletePropertyAsync(Guid id)
        {
            var property = await _db.Propertys.FindAsync(id);
            if (property == null)
                return BaseResponse.Fail("Property not found.");

            var userProfiles = await _db.UserProfiles.Where(u => u.LocationId == id).ToListAsync();
            foreach (var user in userProfiles)
            {
                user.LocationId = null;
            }

            _db.Propertys.Remove(property);
            await _db.SaveChangesAsync();

            await _membershipServiceClient.DeletePropertyAsync(property.Id); 

            return BaseResponse.Ok("Property deleted successfully.");
        }


    }
}
