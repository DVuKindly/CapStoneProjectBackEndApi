using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Services.Interfaces
{
    public interface IEcosystemService
    {
        Task<IEnumerable<EcosystemResponseDto>> GetAllAsync();
        Task<EcosystemResponseDto?> GetByIdAsync(Guid id);
        Task<EcosystemResponseDto> CreateAsync(EcosystemRequestDto dto);
        Task<bool> UpdateAsync(Guid id, EcosystemRequestDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
