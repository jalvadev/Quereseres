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
            }catch(Exception ex)
            {
                Log.Error($"Error getting room by ID in DB: {ex.Message}");
                room = null;
            }

            return room;
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
