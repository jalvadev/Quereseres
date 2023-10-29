using API.Quereseres.Models;

namespace API.Quereseres.Interfaces
{
    public interface IHouseworkWeeklyRepository : IDisposable
    {
        public HouseworkWeekly GetHouseworkWeeklyById(int id);

        public List<HouseworkWeekly> GetHouseworkWeeklyByRoomId(int roomId);

        public List<HouseworkWeekly> GetHouseworkWeeklyByUserId(int userId);

        public HouseworkWeekly CreateHouseworkWeekly(HouseworkWeekly houseworkWeekly);

        public List<HouseworkWeekly> CreateAllHouseworkWeeklyByHouseId(int houseId, DateTime limitDate);

        void Save();
    }
}
