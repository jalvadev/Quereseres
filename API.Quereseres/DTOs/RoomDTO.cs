namespace API.Quereseres.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HouseId { get; set; }

        public List<HouseworkDTO>? HouseworkList { get; set; }
    }
}
