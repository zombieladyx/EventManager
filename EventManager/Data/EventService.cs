using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class EventService:IEventService
    {
        private readonly EventManagerContext _context;

        public EventService(EventManagerContext context) => _context = context;

        public async Task<List<Event>> GetEventsAsync() => await _context.Events.ToListAsync();

        public async Task<Event?> GetEventAsync(string id) => await _context.Events.FindAsync(id);

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

        public async Task DeleteEventAsync(string id)
        {
            var ev = await GetEventAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
        }

        public async Task JoinEventAsync(string userId, string eventId)
        {
            if (!await IsUserJoined(userId, eventId))
            {
                var eventUser = new User_Event { Email = userId, EVENT_ID = eventId };
                _context.User_Event.Add(eventUser);
                await _context.SaveChangesAsync();

                // Zwiększ CURR_SPACE
                var ev = await _context.Events.FindAsync(eventId);
                if (ev != null)
                {
                    ev.CURR_SPACE++;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task LeaveEventAsync(string userId, string eventId)
        {
            var eventUser = await _context.User_Event
                .FirstOrDefaultAsync(eu => eu.Email == userId && eu.EVENT_ID == eventId);

            if (eventUser != null)
            {
                _context.User_Event.Remove(eventUser);
                await _context.SaveChangesAsync();

                var ev = await _context.Events.FindAsync(eventId);
                if (ev != null)
                {
                    ev.CURR_SPACE--;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> IsUserJoined(string userId, string eventId) =>
            await _context.User_Event.AnyAsync(eu => eu.Email == userId && eu.EVENT_ID == eventId);

        public async Task<List<User>> GetEventUsersAsync(string eventId) =>
            await _context.User_Event
                .Where(eu => eu.EVENT_ID == eventId)
                .Include(eu => eu.User)
                .Select(eu => eu.User)
                .ToListAsync();
    }
}
