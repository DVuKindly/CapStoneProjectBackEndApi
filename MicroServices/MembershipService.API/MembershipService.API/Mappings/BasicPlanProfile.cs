using AutoMapper;
using MembershipService.API.Common.Constants;
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
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlan, opt => opt.Ignore());

            CreateMap<PackageDurationDto, ComboPlanDuration>()
                .ForMember(dest => dest.PackageDurationId, opt => opt.MapFrom(src => src.DurationId))
                .ForMember(dest => dest.DiscountDurationRate, opt => opt.MapFrom(src => src.DiscountRate));

            CreateMap<PlanEntitlementDto, BasicPlanEntitlement>()
                .ForMember(dest => dest.EntitlementRuleId, opt => opt.MapFrom(src => src.EntitlementRuleId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.BasicPlan, opt => opt.Ignore())
                .ForMember(dest => dest.EntitlementRule, opt => opt.Ignore());


            CreateMap<CreateBasicPlanRequest, BasicPlan>()
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Convert.ToDecimal(src.Price)))
                .ForMember(dest => dest.VerifyBuy, opt => opt.MapFrom(src => src.VerifyBuy))
                .ForMember(dest => dest.RequireBooking, opt => opt.MapFrom(src => src.RequireBooking))

                // ❌ Bỏ ánh xạ các collection → sẽ xử lý thủ công trong service
                .ForMember(dest => dest.BasicPlanRooms, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanEntitlements, opt => opt.Ignore())
                .ForMember(dest => dest.ComboPlanDurations, opt => opt.Ignore())

                // Các navigation khác
                .ForMember(dest => dest.BasicPlanType, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanCategory, opt => opt.Ignore())
                .ForMember(dest => dest.BasicPlanLevel, opt => opt.Ignore())
                .ForMember(dest => dest.PlanTargetAudience, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.ComboPlanBasics, opt => opt.Ignore())
                .ForMember(dest => dest.Memberships, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipHistories, opt => opt.Ignore());







            CreateMap<UpdateBasicPlanRequest, BasicPlan>();

            // Mapping từ entity → response DTO
            CreateMap<BasicPlan, BasicPlanResponseDto>()
                .ForMember(dest => dest.BasicPlanType, opt => opt.MapFrom(src => src.BasicPlanType != null ? src.BasicPlanType.Name : null))
                .ForMember(dest => dest.BasicPlanTypeCode, opt => opt.MapFrom(src => src.BasicPlanType != null ? src.BasicPlanType.Code : null))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : null))
                .ForMember(dest => dest.PlanLevelName, opt => opt.MapFrom(src => src.BasicPlanLevel != null ? src.BasicPlanLevel.Name : null))
                .ForMember(dest => dest.TargetAudienceName, opt => opt.MapFrom(src => src.PlanTargetAudience != null ? src.PlanTargetAudience.Name : null))
                .ForMember(dest => dest.PlanCategoryName, opt => opt.MapFrom(src => src.BasicPlanCategory != null ? src.BasicPlanCategory.Name : null))
                .ForMember(dest => dest.PlanDurations, opt => opt.MapFrom(src => src.ComboPlanDurations))

                // Nếu là loại Accomodation → map sang danh sách Acomodations
                .ForMember(dest => dest.Acomodations, opt => opt.MapFrom(src =>
                    src.BasicPlanType.Code == BasicPlanTypeCodes.Accommodation
                        ? src.BasicPlanRooms.Select(op => new BasicPlanRoomResponseDto
                        {
                            AccomodationId = op.AccommodationOptionId,
                            AccomodationDescription = op.AccommodationOption.Description,
                            RoomType = op.AccommodationOption.RoomType.Name
                        }).ToList()
                        : new List<BasicPlanRoomResponseDto>()
                ))

                // Nếu KHÔNG phải loại Accomodation → map sang danh sách Entitlements
                .ForMember(dest => dest.Entitlements, opt => opt.MapFrom(src =>
                    src.BasicPlanType.Code != BasicPlanTypeCodes.Accommodation
                        ? src.BasicPlanEntitlements.Select(e => new EntitlementResponseDto
                        {
                            EntitlementId = e.EntitlementRuleId,
                            NextUSerName = e.EntitlementRule.NextUService.Name // tùy theo navigation
                        }).ToList()
                        : new List<EntitlementResponseDto>()
                ));



            CreateMap<ComboPlanDuration, PlanDurationResponseDto>()
                .ForMember(dest => dest.PlanDurationId, opt => opt.MapFrom(src => src.PackageDurationId))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DiscountDurationRate))
                .ForMember(dest => dest.PlanDurationUnit, opt => opt.MapFrom(src => src.PackageDuration.Unit))
                .ForMember(dest => dest.PlanDurationValue, opt => opt.MapFrom(src => src.PackageDuration.Value.ToString()))
                .ForMember(dest => dest.PlanDurationDescription, opt => opt.MapFrom(src => src.PackageDuration.Description));

            //CreateMap<BasicPlanRoom, BasicPlanRoomResponseDto>()
        }

        private static string? GetAccommodationDescription(BasicPlan src)
        {
            return src.BasicPlanRooms?
                      .FirstOrDefault()?
                      .AccommodationOption?
                      .Description;
        }
        private static Guid? GetAccommodationId(BasicPlan src)
        {
            return src.BasicPlanRooms?
                      .FirstOrDefault()?
                      .AccommodationOption?
                      .Id;
        }


    }
}
