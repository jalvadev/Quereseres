using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API.Quereseres.Repositories
{
    public class HomeRepository : IHomeRepository, IDisposable
    {
        private readonly QuereseresContext _context;
        private bool _disposed;

        public HomeRepository(QuereseresContext context)
        {
            _context = context;
            _disposed = false;
        }

        public House InsertHome(House home)
        {
            House newHome;

            try
            {
                var result = _context.Houses.Add(home);
                newHome = result.Entity;
                Save();
            }
            catch(Exception ex) 
            {
                Log.Error($"Error inserting home in DB: {ex.Message}");
                newHome = null;
            }
            

            return newHome;
        }

        public House GetHouseByUserId(int userId)
        {
            House? house;

            try
            {
                house = null;

                var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();

                if(user != null)
                    house = _context.Houses.Where(h => h.UserList.Contains(user)).FirstOrDefault();

                if(house != null)
                    house.UserList = _context.Users.Where(u => u.House != null && u.House.Id == house.Id).ToList();
            }
            catch(Exception ex)
            {
                Log.Error($"Error getting home list by user from DB: {ex.Message}");
                house = null;
            }

            return house;
        }

        public House GetHomeByIdAndUser(int homeId, User user)
        {
            House home;

            try
            {
                var houses = _context.Houses.Where(h => h.Id == homeId);

                // filtramos las casas por el id del usuario.
                houses = houses.Where(h => h.UserList.Contains(user));

                home = houses.FirstOrDefault();
            }
            catch(Exception ex)
            {
                Log.Error($"Error getting home by user from DB: {ex.Message}");
                home = null;
            }

            return home;
        }

        public bool CheckHomeByIdAndUserEmail(int homeId, string email)
        {
            bool exists = true;

            try
            {
                var houseUsers = _context.Houses.Where(h => h.Id == homeId).Select(h => h.UserList).FirstOrDefault();
                if (houseUsers == null)
                {
                    exists = false;
                    return exists;
                }

                var user = houseUsers.Where(u => u.Email == email).FirstOrDefault();
                if (user == null)
                {
                    exists = false;
                    return exists;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"Error checking user/home from DB: {ex.Message}");
                exists = false;
            }

            return exists;
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
