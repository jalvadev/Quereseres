using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Quereseres.Repositories
{
    public class HouseworkRepository : IHouseworkRepository
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
                Log.Error($"Error getting housework by ID in DB: {ex.Message}");
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
                Log.Error($"Error getting housework by roomId in DB: {ex.Message}");
                houseworkList = null;
            }

            return houseworkList;
        }

        public List<Housework> ListHouswork(int houseId)
        {
            List<Housework> houseworkList;

            try
            {
                houseworkList = _context.Houseworks.Where(h => h.Room.House.Id == houseId).ToList();
            }catch(Exception ex)
            {
                Log.Error($"Error getting housework by house in DB: {ex.Message}");
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
                Log.Error($"Error inserting new housework in DB: {ex.Message}");
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
