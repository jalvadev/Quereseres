namespace API.Quereseres.DTOs
{
    public class HouseworkWeeklyDTO
    {
        public int Id { get; set; }

        public int houseworkId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public string RoomName { get; set; }

        public int RoomId { get; set; }

        public bool IsDone { get; set; }

        public DateTime LimitDate { get; set; }
    }
}
