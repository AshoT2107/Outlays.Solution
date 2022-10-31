using Outlays.Data.Enums;

namespace Outlays.Data.Models
{
    public class GetRoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public RoomStatus Status { get; set; }
        public GetUserModel? Admin { get; set; }
    }
}
