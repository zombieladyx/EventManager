namespace EventManager.Data
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync();
        Task<Event?> GetEventAsync(string id);
        Task AddEventAsync(Event en);
        Task UpdateEventAsync(Event en);
        Task DeleteEventAsync(string id);
        Task JoinEventAsync(string userId, string eventId);
        Task LeaveEventAsync(string userId, string eventId);
        Task<bool> IsUserJoined(string userId, string eventId);
        Task<List<User>> GetEventUsersAsync(string eventId);
    }

}
