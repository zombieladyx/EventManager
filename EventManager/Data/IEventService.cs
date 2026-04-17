namespace EventManager.Data
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync();
        Task<Event?> GetEventAsync(int  id);
        Task AddEventAsync(Event en);
        Task UpdateEventAsync(Event en);
        Task DeleteEventAsync(int id);
    }
}
