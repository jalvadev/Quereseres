using API.Quereseres.Models;

namespace API.Quereseres.Interfaces
{
    public interface IRoomRepository : IDisposable
    {
        public Room GetRoomById(int roomId);

        public List<Room> GetRoomListByHouseId(int houseId);

        public Room InsertRoom(Room room);

        void Save();
    }
}
