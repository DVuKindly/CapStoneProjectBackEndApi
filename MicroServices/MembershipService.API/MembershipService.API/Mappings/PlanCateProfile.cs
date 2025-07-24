using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class PlanCateProfile : Profile
    {
        public PlanCateProfile()
        {
            // PlanCategory
            CreateMap<PlanCategory, PlanCategoryDto>();

            // PlanLevel
            CreateMap<PlanLevel, PlanLevelDto>();

            // PlanTargetAudience
            CreateMap<PlanTargetAudience, PlanTargetAudienceDto>();
        }
    }
}