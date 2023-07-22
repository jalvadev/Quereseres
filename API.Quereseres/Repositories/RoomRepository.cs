using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;

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

        public Room InsertRoom(Room room)
        {
            var newRoom = _context.Rooms.Add(room);
            Save();

            return newRoom.Entity;
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
