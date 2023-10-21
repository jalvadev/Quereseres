namespace API.Quereseres.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Housework> HouseworkList { get; set; }

    }
}
