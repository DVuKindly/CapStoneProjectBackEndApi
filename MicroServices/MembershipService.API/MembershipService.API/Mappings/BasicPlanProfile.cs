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
                .ForMember(dest => dest.AccommodationOptionId, opt => opt.MapFrom(src => src.AccomodationId))
                .ForMember(dest => dest.RoomInstanceId, opt => opt.MapFrom(src => src.RoomInstanceId))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src =>
                    src.TotalPrice ?? (src.CustomPricePerNight.HasValue
                        ? src.CustomPricePerNight.Value * src.NightsIncluded
                        : 0)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlan, opt => opt.Ignore());

            CreateMap<PackageDurationDto, ComboPlanDuration>()
                .ForMember(dest => dest.PackageDurationId, opt => opt.MapFrom(src => src.DurationId))
                .ForMember(dest => dest.DiscountDurationRate, opt => opt.MapFrom(src => src.DiscountRate));

            CreateMap<CreateBasicPlanRequest, BasicPlan>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Convert.ToDecimal(src.Price)))
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanType, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanCategory, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanLevel, opt => opt.Ignore())
                .ForMember(dest => dest.PlanTargetAudience, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanRooms, opt => opt.MapFrom(src => src.Rooms))
                .ForMember(dest => dest.BasicPlanEntitlements, opt => opt.Ignore())
                .ForMember(dest => dest.ComboPlanBasics, opt => opt.Ignore())
                .ForMember(dest => dest.Memberships, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipHistories, opt => opt.Ignore())
                .ForMember(dest => dest.ComboPlanDurations, opt => opt.Ignore());

            CreateMap<UpdateBasicPlanRequest, BasicPlan>();

            // Mapping từ entity → response DTO
            CreateMap<BasicPlan, BasicPlanResponseDto>()
               .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null))
               .ForMember(dest => dest.BasicPlanType, opt => opt.MapFrom(src => src.BasicPlanType != null ? src.BasicPlanType.Name : null))
               .ForMember(dest => dest.PlanLevelName, opt => opt.MapFrom(src => src.BasicPlanLevel != null ? src.BasicPlanLevel.Name : null))
               .ForMember(dest => dest.TargetAudienceName, opt => opt.MapFrom(src => src.PlanTargetAudience != null ? src.PlanTargetAudience.Name : null))
               .ForMember(dest => dest.PlanCategoryName, opt => opt.MapFrom(src => src.BasicPlanCategory != null ? src.BasicPlanCategory.Name : null))
               .ForMember(dest => dest.PlanDurations, opt => opt.MapFrom(src => src.ComboPlanDurations))
               .ForMember(dest => dest.AccomodationDescription, opt => opt.MapFrom(src => GetAccommodationDescription(src)))
               .ForMember(dest => dest.AccomodationId, opt => opt.MapFrom(src => GetAccommodationIdFlexible(src)));

            CreateMap<ComboPlanDuration, PlanDurationResponseDto>()
                .ForMember(dest => dest.PlanDurationId, opt => opt.MapFrom(src => src.PackageDurationId))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DiscountDurationRate))
                .ForMember(dest => dest.PlanDurationUnit, opt => opt.MapFrom(src => src.PackageDuration.Unit))
                .ForMember(dest => dest.PlanDurationValue, opt => opt.MapFrom(src => src.PackageDuration.Value.ToString()))
                .ForMember(dest => dest.PlanDurationDescription, opt => opt.MapFrom(src => src.PackageDuration.Description));

            CreateMap<BasicPlanRoom, BasicPlanRoomResponseDto>()
                .ForMember(dest => dest.RoomInstanceId, opt => opt.MapFrom(src => src.RoomInstanceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoomInstance != null ? src.RoomInstance.RoomCode : string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.RoomInstance != null ? src.RoomInstance.DescriptionDetails : string.Empty))
                .ForMember(dest => dest.NightsIncluded, opt => opt.MapFrom(src => src.NightsIncluded))
                .ForMember(dest => dest.CustomPricePerNight, opt => opt.MapFrom(src => src.CustomPricePerNight))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
        }

        private static string? GetAccommodationDescription(BasicPlan src)
        {
            var room = src.BasicPlanRooms?.FirstOrDefault();

          
            if (room?.RoomInstance?.AccommodationOption != null)
            {
                return room.RoomInstance.AccommodationOption.Description;
            }

            // Nếu chưa có RoomInstance → lấy từ navigation AccommodationOption (nếu đã Include)
            return room?.AccommodationOption?.Description;
        }


        // Ưu tiên lấy từ RoomInstance.AccommodationOption.Id, nếu không thì fallback sang AccommodationOptionId
        private static Guid? GetAccommodationIdFlexible(BasicPlan src)
        {
            var room = src.BasicPlanRooms?.FirstOrDefault();

            if (room?.RoomInstance?.AccommodationOption != null)
                return room.RoomInstance.AccommodationOption.Id;

            return room?.AccommodationOptionId;
        }
    }
}
