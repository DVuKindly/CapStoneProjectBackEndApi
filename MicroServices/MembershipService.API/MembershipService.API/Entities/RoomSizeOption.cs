namespace MembershipService.API.Entities
{
    public class RoomSizeOption
    {
        public int Id { get; set; } // 1=Nhỏ, 2=Vừa, 3=Lớn
        public string Name { get; set; } = null!;
        public ICollection<RoomInstance> Rooms { get; set; } = new List<RoomInstance>();

    }
}
