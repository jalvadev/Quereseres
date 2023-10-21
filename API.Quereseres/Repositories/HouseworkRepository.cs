using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Quereseres.Repositories
{
    public class HouseworkRepository : IHouseworkRepository, IDisposable
    {
        private readonly QuereseresContext _context;
        private bool _disposed;

        public HouseworkRepository(QuereseresContext context)
        {
            _context = context;
        }

        public Housework GetHouseworkById(int houseworkId)
        {
            Housework housework;

            try
            {
                housework = _context.Houseworks.Where(h => h.Id == houseworkId).FirstOrDefault();
            }
            catch(Exception ex) 
            {
                // TODO: Add logger.
                housework = null;
            }

            return housework;
        }

        public List<Housework> GetHouseworksByRoomId(int roomId)
        {
            List<Housework> houseworkList;

            try
            {
                houseworkList = _context.Houseworks.Where(h => h.Room.Id == roomId).ToList();
            }
            catch (Exception ex)
            {
                // TODO: Add logger.
                houseworkList = null;
            }

            return houseworkList;
        }

        public Housework InsertHousework(Housework housework)
        {
            try
            {
                var newHousework = _context.Houseworks.Add(housework);
                Save();

                return newHousework.Entity;
            }catch(Exception ex)
            {
                // TODO: Add logger.
                return null;
            }
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
