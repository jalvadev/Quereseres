using Microsoft.IdentityModel.Protocols;
using Quereseres.Contexts;
using Quereseres.Interfaces;
using Quereseres.Models;

namespace Quereseres.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly QuereseresContext _context;
        private bool _disposed;
        
        public UserRepository(QuereseresContext context)
        {
            _context = context;
            _disposed = false;
        }

        public User GetUserByCredentials(string email, string password)
        {
            return _context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
        }

        public User GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
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
