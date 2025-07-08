    namespace UserService.API.Entities
    {
        public abstract class AuditableEntity
        {
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime? UpdatedAt { get; set; }
            public Guid? CreatedBy { get; set; }
            public Guid? UpdatedBy { get; set; }
        }
    }
