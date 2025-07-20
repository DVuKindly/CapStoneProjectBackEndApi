namespace MembershipService.API.Entities
{
    public class BedTypeOption
    {
        public int Id { get; set; } // 1=Đơn, 2=Đôi,...
        public string Name { get; set; } = null!;
        public ICollection<RoomInstance> Rooms { get; set; } = new List<RoomInstance>();

    }
}
