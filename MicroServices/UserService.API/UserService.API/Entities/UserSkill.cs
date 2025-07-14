namespace UserService.API.Entities
{
    public class UserSkill
    {
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } // Bạn cần tạo thêm bảng Skill
    }
}
