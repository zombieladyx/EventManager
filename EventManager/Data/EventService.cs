using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    // Serwis odpowiedzialny za operacje CRUD na encji Event
    // oraz za dołączanie i opuszczanie eventów przez użytkowników
    public class EventService : IEventService
    {
        // Kontekst bazy danych używany do odczytu i zapisu danych
        private readonly EventManagerContext _context;

        // Wstrzyknięcie DbContext przez Dependency Injection
        public EventService(EventManagerContext context) => _context = context;

        // Pobiera wszystkie eventy z bazy danych
        public async Task<List<Event>> GetEventsAsync() => await _context.Events.ToListAsync();

        // Pobiera jeden event po jego identyfikatorze
        public async Task<Event?> GetEventAsync(string id) => await _context.Events.FindAsync(id);

        // Dodaje nowy event do bazy danych
        public async Task AddEventAsync(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
        }

        // Aktualizuje istniejący event w bazie danych
        public async Task UpdateEventAsync(Event ev)
        {
            _context.Entry(ev).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Usuwa event z bazy danych na podstawie identyfikatora
        public async Task DeleteEventAsync(string id)
        {
            var ev = await GetEventAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
        }

        // Dodaje powiązanie użytkownika z eventem, jeśli jeszcze nie istnieje
        public async Task JoinEventAsync(string userId, string eventId)
        {
            // Sprawdzenie, czy użytkownik nie jest już zapisany na ten event
            if (!await IsUserJoined(userId, eventId))
            {
                // Utworzenie rekordu w tabeli pośredniej user-event
                var eventUser = new User_Event { Email = userId, EVENT_ID = eventId };
                _context.User_Event.Add(eventUser);
                await _context.SaveChangesAsync();

                // Zwiększenie liczby zajętych miejsc po dołączeniu użytkownika
                var ev = await _context.Events.FindAsync(eventId);
                if (ev != null)
                {
                    ev.CURR_SPACE++;
                    await _context.SaveChangesAsync();
                }
            }
        }

        // Usuwa powiązanie użytkownika z eventem
        public async Task LeaveEventAsync(string userId, string eventId)
        {
            // Szukanie rekordu powiązania użytkownika z eventem
            var eventUser = await _context.User_Event
                .FirstOrDefaultAsync(eu => eu.Email == userId && eu.EVENT_ID == eventId);

            if (eventUser != null)
            {
                // Usunięcie powiązania z tabeli pośredniej
                _context.User_Event.Remove(eventUser);
                await _context.SaveChangesAsync();

                // Zmniejszenie liczby zajętych miejsc po opuszczeniu eventu
                var ev = await _context.Events.FindAsync(eventId);
                if (ev != null)
                {
                    ev.CURR_SPACE--;
                    await _context.SaveChangesAsync();
                }
            }
        }

        // Sprawdza, czy dany użytkownik jest już zapisany na event
        public async Task<bool> IsUserJoined(string userId, string eventId) =>
            await _context.User_Event.AnyAsync(eu => eu.Email == userId && eu.EVENT_ID == eventId);

        // Pobiera wszystkich użytkowników zapisanych na dany event
        public async Task<List<User>> GetEventUsersAsync(string eventId) =>
            await _context.User_Event
                .Where(eu => eu.EVENT_ID == eventId)
                .Include(eu => eu.User)
                .Select(eu => eu.User)
                .ToListAsync();
    }
}