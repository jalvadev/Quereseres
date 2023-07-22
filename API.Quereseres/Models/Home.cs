namespace API.Quereseres.Models
{
    public class Home
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime RecordInitDate { get; set; }

        public List<User> UserList { get; set; }
    }
}
