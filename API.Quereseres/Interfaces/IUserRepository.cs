using API.Quereseres.Models;

namespace API.Quereseres.Interfaces
{
    public interface IUserRepository : IDisposable
    {

        User GetUserById(int id);

        User GetUserByCredentials(string email, string password);

        public User GetUserByEmail(string email);

        void Save();
    }
}
