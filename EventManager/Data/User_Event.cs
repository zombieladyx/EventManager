namespace EventManager.Data
{
    // Encja pośrednia reprezentująca zapis użytkownika na event
    // Służy do przechowywania relacji many-to-many pomiędzy User i Event
    public class User_Event
    {
        // Email użytkownika będący kluczem obcym do tabeli User
        public required string Email { get; set; }

        // Id eventu będące kluczem obcym do tabeli Event
        public required string EVENT_ID { get; set; }

        // Nawigacja do obiektu użytkownika powiązanego z tym wpisem
        public User User { get; set; } = null!;

        // Nawigacja do obiektu eventu powiązanego z tym wpisem
        public Event Event { get; set; } = null!;
    }
}