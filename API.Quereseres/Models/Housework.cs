namespace API.Quereseres.Models
{
    public class Housework
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public User AssignedUser { get; set; }

        public Room Room { get; set; }
    }
}
