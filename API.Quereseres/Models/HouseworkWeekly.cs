namespace API.Quereseres.Models
{
    public class HouseworkWeekly
    {
        public int Id { get; set; }

        public Housework Housework { get; set; }

        public DateTime LimitDate { get; set; }

        public bool IsDone { get; set; }
    }
}
