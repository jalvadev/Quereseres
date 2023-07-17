using Quereseres.Models;

namespace Quereseres.Interfaces
{
    public interface IUserRepository : IDisposable
    {

        User GetUserById(int id);

        User GetUserByCredentials(string email, string password);

        void Save();
    }
}
