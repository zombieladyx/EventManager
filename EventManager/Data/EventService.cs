using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class EventService:IEventService
    {
        private readonly EventManagerContext _context;

        public EventService(EventManagerContext context) => _context = context;

        public async Task<List<Event>> GetEventsAsync() => await _context.Events.ToListAsync();

        public async Task<Event?> GetEventAsync(int id) => await _context.Events.FindAsync(id);

        public async Task AddEventAsync(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event ev)
        {
            _context.Entry(ev).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var ev = await GetEventAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
        }
    }
}
