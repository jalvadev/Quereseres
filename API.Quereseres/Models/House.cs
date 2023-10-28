namespace API.Quereseres.Models
{
    public class House
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DayOfWeek LimitDay { get; set; }

        public List<User> UserList { get; set; }

        public List<Room> RoomList { get; set; }
    }

    internal enum DaysOfWeek
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
    }
}
