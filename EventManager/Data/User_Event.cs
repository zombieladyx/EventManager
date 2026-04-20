namespace EventManager.Data
{
    public class User_Event
    {
        public required string Email { get; set; }
        public required string EventId { get; set; }
        public User User { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}
