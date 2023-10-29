using API.Quereseres.Models;

namespace API.Quereseres.DTOs
{
    public class HouseworkDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int HouseId { get; set; }

        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public UserDTO? AssignedUser { get; set; }

    }
}
