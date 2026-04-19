namespace EventManager.Data
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync();
        Task<Event?> GetEventAsync(string id);
        Task AddEventAsync(Event en);
        Task UpdateEventAsync(Event en);
        Task DeleteEventAsync(string id);
    }
}
