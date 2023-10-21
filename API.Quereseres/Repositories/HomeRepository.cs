using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;

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

        public Home InsertHome(Home home)
        {
            var newHome = _context.Houses.Add(home);
            Save();

            return newHome.Entity;
        }

        public List<Home> GetUserHomes(int userId)
        {
            List<Home> homeList;

            try
            {
                homeList = _context.Houses
                    .Join(_context.Users,
                        houses => userId,
                        users => userId,
                        (houses, users) => houses)
                    .Where(user => user.Id == userId)
                    .ToList();
            }catch(Exception ex)
            {
                // TODO: Add logger.
                homeList = null;
            }

            return homeList;
        }

        public Home GetHomeByIdAndUser(int homeId, User user)
        {
            // obtenemos la casa por id.
            var home = _context.Houses.Where(h => h.Id == homeId);

            // filtramos las casas por el id del usuario.
            home = home.Where(h => h.UserList.Contains(user));

            return home.FirstOrDefault();
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
                // TODO: Add logger.
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
