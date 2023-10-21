using API.Quereseres.Context;
using API.Quereseres.Interfaces;
using API.Quereseres.Models;

namespace API.Quereseres.Repositories
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
            User user;

            try
            {
                user = _context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
            }catch(Exception ex)
            {
                // TODO: Add logger.
                user = null;
            }

            return user;
        }

        public User GetUserById(int id)
        {
            User user;

            try
            {
                user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            }catch (Exception ex)
            {
                // TODO: Add logger.
                user = null;
            }

            return user;
        }

        public User GetUserByEmail(string email)
        {
            User user;

            try
            {
                user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // TODO: Add logger.
                user = null;
            }

            return user;
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
