namespace EventManager.Data
{
    // Klasa reprezentująca pojedynczy event w aplikacji
    public class Event
    {
        // Unikalny identyfikator eventu
        public string EVENT_ID { get; set; } = string.Empty;

        // Krótki opis eventu, wyświetlany np. na liście
        public string SHORT_DESC { get; set; } = string.Empty;

        // Pełny opis eventu
        public string DESCRIPTION { get; set; } = string.Empty;

        // Maksymalna liczba miejsc dostępnych na evencie
        public int SPACE_LIMIT { get; set; }

        // Aktualna liczba zajętych miejsc
        public int CURR_SPACE { get; set; }

        // Data odbycia się eventu
        public DateOnly EVENT_DATE { get; set; }

        // Kolekcja powiązań między eventem a użytkownikami, którzy do niego dołączyli
        public ICollection<User_Event> EventUsers { get; set; } = new List<User_Event>();
    }
}