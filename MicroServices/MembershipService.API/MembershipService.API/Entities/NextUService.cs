﻿using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;

namespace MembershipService.API.Entities
{
    public class NextUService : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ServiceType ServiceType { get; set; }

        public Guid EcosystemId { get; set; }
        public Ecosystem Ecosystem { get; set; }
        public Guid? PropertyId { get; set; }
        public Property? Property { get; set; } = null!;

        public ICollection<AccommodationOption> AccommodationOptions { get; set; }
        public ICollection<EntitlementRule> EntitlementRules { get; set; }

    }
}