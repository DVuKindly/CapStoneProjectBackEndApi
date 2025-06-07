namespace AuthService.API.Entities
{
    public static class SystemRoles
    {
        public static readonly Guid SuperAdmin = Guid.Parse("CCA492D0-42B4-42B4-A3EC-4265A521B5F8");
        public static readonly Guid Admin = Guid.Parse("7B6C3929-7BCE-48E8-9F60-B0DF90792C5C");
        public static readonly Guid Manager = Guid.Parse("05387973-58B6-48D1-9CC4-11874D712149");
        public static readonly Guid StaffOnboarding = Guid.Parse("F5D32824-116A-4F97-AE91-73C7D84C6486");
        public static readonly Guid StaffService = Guid.Parse("D9F65410-6925-4351-A5AD-4AAAF5DF0305");
        public static readonly Guid StaffContent = Guid.Parse("833D3494-42B4-2222-AFC2-4265A521C5F8");

        public static readonly Guid User = Guid.Parse("ED3A2AE2-F05B-47E6-949F-28FB477D5B24");
        public static readonly Guid Member = Guid.Parse("78454018-BEC6-459D-85E2-4265A521B5F8");
        public static readonly Guid Partner = Guid.Parse("CCA492D0-4DE1-42B4-AFC2-BB613EEB219B");
        public static readonly Guid Coaching = Guid.Parse("833D5494-3559-4826-A3EC-58B0B14C7C81");
        public static readonly Guid Supplier = Guid.Parse("833D5492-AFC2-42B4-AFC2-8265A521B5F8");
    }
}
