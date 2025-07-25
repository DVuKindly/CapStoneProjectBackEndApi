﻿using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class BasicPlan : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }
        public bool RequireBooking { get; set; } = false;

        public Guid BasicPlanTypeId { get; set; }
        public BasicPlanType BasicPlanType { get; set; }

        public int BasicPlanCategoryId { get; set; }
        public PlanCategory BasicPlanCategory { get; set; }

        public int PlanLevelId { get; set; }
        public PlanLevel BasicPlanLevel { get; set; }

        public int TargetAudienceId { get; set; }
        public PlanTargetAudience PlanTargetAudience { get; set; }

        public Guid? PropertyId { get; set; }
        public Property? Property { get; set; } = null!;

        // Danh sách quyền lợi nếu là plan hằng ngày
        public ICollection<BasicPlanEntitlement>? BasicPlanEntitlements { get; set; }

        // Danh sách phòng nếu là plan lưu trú
        public ICollection<BasicPlanRoom>? BasicPlanRooms { get; set; }

        // Các liên kết phụ
        public ICollection<ComboPlanDuration> ComboPlanDurations { get; set; }
        public ICollection<ComboPlanBasic> ComboPlanBasics { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<MembershipHistory> MembershipHistories { get; set; }


    }
}