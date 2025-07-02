using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class BasicPackageProfile : Profile
    {
        public BasicPackageProfile()
        {
            // Mapping từ request DTOs → entity
            CreateMap<CreateBasicPlanRequest, BasicPlan>();
            CreateMap<UpdateBasicPlanRequest, BasicPlan>();

            // Mapping từ entity → response DTO
            CreateMap<BasicPlan, BasicPlanResponseDto>()
                .ForMember(dest => dest.ServiceIds, opt => opt.Ignore()) // Tạm ignore nếu có logic riêng để gán
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null))
                .ForMember(dest => dest.BasicPlanType, opt => opt.MapFrom(src => src.BasicPlanType != null ? src.BasicPlanType.Name : null))
                .ForMember(dest => dest.PlanLevelName, opt => opt.MapFrom(src => src.BasicPlanLevel != null ? src.BasicPlanLevel.Name : null))
                .ForMember(dest => dest.TargetAudienceName, opt => opt.MapFrom(src => src.PlanTargetAudience != null ? src.PlanTargetAudience.Name : null))
                .ForMember(dest => dest.PlanCategoryName, opt => opt.MapFrom(src => src.BasicPlanCategory != null ? src.BasicPlanCategory.Name : null));
        }
    }
}
