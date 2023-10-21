using API.Quereseres.Models;

namespace API.Quereseres.Interfaces
{
    public interface IHouseworkRepository : IDisposable
    {
        public List<Housework> GetHouseworksByRoomId(int roomId);

        public Housework GetHouseworkById(int houseworkId);

        public Housework InsertHousework(Housework housework);

        void Save();
    }
}
