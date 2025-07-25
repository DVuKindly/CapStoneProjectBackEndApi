﻿using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPlanRepository
    {
        Task<BasicPlan> AddAsync(BasicPlan entity);
        Task<BasicPlan> UpdateAsync(BasicPlan entity);
        Task<bool> ExistsByCodeAsync(string code);

        Task<BasicPlan?> GetByIdAsync(Guid id);
        Task<List<BasicPlanResponseDto>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
        Task<List<BasicPlanResponseDto>> GetByTypeIdAsync(Guid typeId);

    }
}