using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using Serilog;

namespace API.Quereseres.Repositories
{
    public class RoomRepository : IRoomRepository, IDisposable
    {
        private readonly QuereseresContext _context;
        private bool _disposed;

        public RoomRepository(QuereseresContext context)
        {
            _context = context;
        }

        public Room GetRoomById(int roomId)
        {
            Room room;

            try
            {
                room = _context.Rooms.Where(r => r.Id == roomId).FirstOrDefault();

                if (room != null)
                    room.House = _context.Houses.Where(h => h.RoomList.Contains(room)).FirstOrDefault();

                if (room != null && room.House != null)
                    room.HouseworkList = _context.Houseworks.Where(hw => hw.Room.Id == room.Id).ToList();

            }catch(Exception ex)
            {
                Log.Error($"Error getting room by ID in DB: {ex.Message}");
                room = null;
            }

            return room;
        }

        public List<Room> GetRoomListByHouseId(int houseId)
        {
            List<Room> roomList;

            try
            {
                roomList = _context.Rooms.Where(r => r.House.Id == houseId).ToList();
            }catch(Exception ex) 
            {
                Log.Error($"Error getting room list by house ID in DB: {ex.Message}");
                roomList = null;
            }

            return roomList;
        }

        public Room InsertRoom(Room newRoom)
        {
            Room room;

            try
            {
                var result = _context.Rooms.Add(newRoom);
                room = result.Entity;

                Save();
            }
            catch(Exception ex)
            {
                Log.Error($"Error inserting new room in DB: {ex.Message}");
                room = null;
            }
            

            return room;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
