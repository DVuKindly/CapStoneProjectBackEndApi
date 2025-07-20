namespace MembershipService.API.Entities
{
    public class RoomFloorOption
    {
        public int Id { get; set; } // 1=Thấp, 2=Trung, 3=Cao,...
        public string Name { get; set; } = null!;
        public ICollection<RoomInstance> Rooms { get; set; } = new List<RoomInstance>();

    }
}
