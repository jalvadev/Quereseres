using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Quereseres.Repositories
{
    public class HouseworkWeeklyRepository : IHouseworkWeeklyRepository
    {
        private readonly QuereseresContext _context;
        private bool _disposed;

        public HouseworkWeeklyRepository(QuereseresContext context)
        {
            _context = context;
        }

        public HouseworkWeekly GetHouseworkWeeklyById(int id)
        {
            HouseworkWeekly houseworkWeekly = null;
            try
            {
                houseworkWeekly = _context.HouseworkWeeklies.Where(h => h.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting houseworkWeekly by ID in DB: {ex.Message}");
                houseworkWeekly = null;
            }

            return houseworkWeekly;
        }

        public List<HouseworkWeekly> GetHouseworkWeeklyByRoomId(int roomId)
        {
            List<HouseworkWeekly> houseworkWeeklyList = null;
            try
            {

                var houseWork = _context.Houseworks.Where(h => h.Room.Id == roomId).ToList();
                houseworkWeeklyList = _context.HouseworkWeeklies.Where(h => houseWork.Contains(h.Housework)).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting houseworkWeekly by room ID in DB: {ex.Message}");
                houseworkWeeklyList = null;
            }

            return houseworkWeeklyList;
        }

        public List<HouseworkWeekly> GetHouseworkWeeklyByUserId(int userId)
        {
            List<HouseworkWeekly> houseworkWeeklyList = null;
            try{

                var houseWork = _context.Houseworks.Where(h => h.AssignedUser.Id == userId).ToList();
                houseworkWeeklyList = _context.HouseworkWeeklies.Where(h => houseWork.Contains(h.Housework)).ToList();
            }catch(Exception ex)
            {
                Log.Error($"Error getting houseworkWeekly by user ID in DB: {ex.Message}");
                houseworkWeeklyList = null;
            }

            return houseworkWeeklyList;
        }

        public HouseworkWeekly CreateHouseworkWeekly(HouseworkWeekly houseworkWeekly)
        {
            try
            {
                var result = _context.Add(houseworkWeekly);
                houseworkWeekly = result.Entity;
            }catch(Exception ex)
            {
                Log.Error($"Error creating houseworkWeekly in DB: {ex.Message}");
                houseworkWeekly = null;
            }

            return houseworkWeekly;
        }

        public List<HouseworkWeekly> CreateAllHouseworkWeeklyByHouseId(int houseId, DateTime limitDate)
        {
            List<HouseworkWeekly> houseworkWeeklyList;
            try
            {
                var houseworkList = _context.Houseworks.Where(r => r.Room.House.Id == houseId).ToList();

                houseworkWeeklyList = houseworkList.Select(h => new HouseworkWeekly { Housework = h, LimitDate = limitDate, IsDone = false }).ToList();

                _context.HouseworkWeeklies.AddRange(houseworkWeeklyList);
            }catch(Exception ex)
            {
                Log.Error($"Error creating houseworkWeekly in range in DB: {ex.Message}");
                houseworkWeeklyList = null;
            }

            return houseworkWeeklyList;
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
