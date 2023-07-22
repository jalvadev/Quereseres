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
