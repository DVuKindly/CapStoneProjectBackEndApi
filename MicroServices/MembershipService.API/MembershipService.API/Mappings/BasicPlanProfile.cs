using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class BasicPlanProfile : Profile
    {
        public BasicPlanProfile()
        {
            // Mapping từ request DTOs → entity
            CreateMap<BasicPlanRoomDto, BasicPlanRoom>()
                .ForMember(dest => dest.RoomInstanceId, opt => opt.MapFrom(src => src.RoomInstanceId))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src =>
                    src.TotalPrice ?? (src.CustomPricePerNight.HasValue
                        ? src.CustomPricePerNight.Value * src.NightsIncluded
                        : 0)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlan, opt => opt.Ignore());

            CreateMap<CreateBasicPlanRequest, BasicPlan>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Convert.ToDecimal(src.Price)))
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanType, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanCategory, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanLevel, opt => opt.Ignore())
                .ForMember(dest => dest.PlanTargetAudience, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanRooms, opt => opt.MapFrom(src => src.Rooms)) // ⬅️ dòng QUAN TRỌNG
                .ForMember(dest => dest.BasicPlanEntitlements, opt => opt.Ignore())
                .ForMember(dest => dest.ComboPlanBasics, opt => opt.Ignore())
                .ForMember(dest => dest.Memberships, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipHistories, opt => opt.Ignore())
                .ForMember(dest => dest.ComboPlanDurations, opt => opt.Ignore());



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
