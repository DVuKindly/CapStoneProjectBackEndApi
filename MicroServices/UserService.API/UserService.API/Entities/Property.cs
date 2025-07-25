﻿    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using UserService.API.Entities;

    [Table("Propertys")]
    public class Property
    {
        [Key]
        public Guid Id { get; set; } 

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? LocationId { get; set; }

    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }

    public virtual ICollection<UserProfile>? UserProfiles { get; set; }
        public virtual ICollection<StaffProfile>? StaffProfiles { get; set; }
        public virtual ICollection<ManagerProfile>? ManagerProfiles { get; set; }
        public virtual ICollection<PartnerProfile>? PartnerProfiles { get; set; }
        public virtual ICollection<PendingMembershipRequest>? PendingMembershipRequests { get; set; }
        public virtual ICollection<PendingThirdPartyRequest>? PendingThirdPartyRequests { get; set; }
    }
