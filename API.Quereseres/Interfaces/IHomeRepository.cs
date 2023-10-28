using API.Quereseres.Models;

namespace API.Quereseres.Interfaces
{
    public interface IHomeRepository : IDisposable
    {
        public House InsertHome(House home);

        public House GetHomeByIdAndUser(int homeId, User user);

        public bool CheckHomeByIdAndUserEmail(int homeId, string email);

        public House GetHouseByUserId(int userId);

        void Save();
    }
}
