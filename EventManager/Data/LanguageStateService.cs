namespace EventManager.Data
{
    public class LanguageStateService          // Klasa przechowująca aktualny język aplikacji.
    {
        public event Action? OnLanguageChanged; // Zdarzenie wywoływane przy zmianie języka.
        private string _currentLanguage = "en"; // Domyślny język ustawiony na angielski.

        // Lista języków, które aplikacja akceptuje.
        private static readonly HashSet<string> AllowedLanguages = new()
        {
            "en", "pl", "de", "es", "ru", "zh-Hans"
        };

        public string CurrentLanguage => _currentLanguage; // Publiczny getter aktualnego języka.

        public void InitializeLanguage(string language)
        {
            if (IsValid(language))              // Sprawdza, czy język jest dozwolony.
                _currentLanguage = language;    // Ustawia język tylko przy starcie aplikacji.
        }

        public void SetLanguage(string language)
        {
            if (!IsValid(language))             // Jeśli język nie jest dozwolony...
                return;                         // ...przerwij bez zmiany.

            _currentLanguage = language;        // Ustaw nowy język.
            OnLanguageChanged?.Invoke();        // Powiadom subskrybentów o zmianie języka.
        }

        // Sprawdza, czy język jest niepusty i znajduje się na liście AllowedLanguages.
        private static bool IsValid(string language) =>
            !string.IsNullOrWhiteSpace(language) && AllowedLanguages.Contains(language);
    }
}
