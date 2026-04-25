namespace EventManager.Data
{
    // Interfejs serwisu odpowiedzialnego za operacje na eventach
    // oraz obsługę zapisu użytkowników do eventów
    public interface IEventService
    {
        // Pobiera listę wszystkich eventów z bazy danych
        Task<List<Event>> GetEventsAsync();

        // Pobiera jeden event po jego identyfikatorze
        Task<Event?> GetEventAsync(string id);

        // Dodaje nowy event do bazy danych
        Task AddEventAsync(Event en);

        // Aktualizuje istniejący event w bazie danych
        Task UpdateEventAsync(Event en);

        // Usuwa event z bazy danych na podstawie identyfikatora
        Task DeleteEventAsync(string id);

        // Dodaje użytkownika do eventu
        Task JoinEventAsync(string userId, string eventId);

        // Usuwa użytkownika z eventu
        Task LeaveEventAsync(string userId, string eventId);

        // Sprawdza, czy dany użytkownik jest już zapisany na dany event
        Task<bool> IsUserJoined(string userId, string eventId);

        // Pobiera listę użytkowników zapisanych na konkretny event
        Task<List<User>> GetEventUsersAsync(string eventId);
    }
}