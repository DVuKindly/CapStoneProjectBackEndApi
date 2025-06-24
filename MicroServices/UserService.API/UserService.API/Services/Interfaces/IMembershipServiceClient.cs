using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IMembershipServiceClient
    {
        Task<BasicPlanDto?> GetBasicPlanByIdAsync(Guid id);

        Task<List<BasicPlanResponse>> GetBasicPlansByIdsAsync(List<Guid> ids);

        // ✅ Sửa kiểu trả về từ int ➜ decimal cho đúng kiểu Price/TotalPrice
        Task<decimal> GetPlanPriceAsync(Guid planId, string planType);

        Task<ComboPlanDto?> GetComboPlanByIdAsync(Guid id);

     
    }
}
