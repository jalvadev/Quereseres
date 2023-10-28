namespace API.Quereseres.DTOs
{
    public class HouseDTO
    {
        public string Name { get; set; }

        public int LimitDay { get; set; }

        public List<UserDTO>? UserList { get; set; }

        public List<RoomDTO>? RoomList { get; set; }
    }
}
