using System.ComponentModel.DataAnnotations;

namespace EventManager.Data
{
    // Klasa reprezentująca pojedynczy event w aplikacji
    public class Event
    {
        // Unikalny identyfikator eventu.
        // Musi mieć dokładnie 6 znaków.
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Id wydarzenia musi mieć dokładnie 6 znaków.")]
        public string EVENT_ID { get; set; } = string.Empty;

        // Krótki opis eventu, np. do wyświetlenia na liście.
        // Nie może przekroczyć 50 znaków.
        [Required]
        [StringLength(50, ErrorMessage = "Krótki opis nie może być dłuższy niż 50 znaków.")]
        public string SHORT_DESC { get; set; } = string.Empty;

        // Pełny opis eventu.
        // Nie może przekroczyć 1000 znaków.
        [Required]
        [StringLength(1000, ErrorMessage = "Pełny opis nie może być dłuższy niż 1000 znaków.")]
        public string DESCRIPTION { get; set; } = string.Empty;

        // Maksymalna liczba miejsc dostępnych na evencie.
        // Wartość musi być większa od 0.
        [Range(1, int.MaxValue, ErrorMessage = "Liczba miejsc musi być większa od 0.")]
        public int SPACE_LIMIT { get; set; }

        // Aktualna liczba zajętych miejsc.
        // Wartość nie może być ujemna.
        [Range(0, int.MaxValue, ErrorMessage = "Liczba zajętych miejsc nie może być ujemna.")]
        public int CURR_SPACE { get; set; }

        // Data odbycia się eventu.
        [Required]
        public DateOnly EVENT_DATE { get; set; }

        // Kolekcja powiązań między eventem a użytkownikami,
        // którzy zapisali się na to wydarzenie.
        public ICollection<User_Event> EventUsers { get; set; } = new List<User_Event>();
    }
}