using System.Text;                 // Udostępnia klasy do pracy z kodowaniem tekstu.
using System.Text.Json;            // Udostępnia klasy do obsługi JSON.

namespace EventManager.Data
{
    public class TranslatorService   // Klasa odpowiedzialna za tłumaczenia przez Azure Translator.
    {
        private readonly HttpClient _httpClient;          // Klient HTTP używany do wysyłania zapytań.
        private readonly IConfiguration _configuration;    // Dostęp do ustawień z appsettings.json.

        public TranslatorService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;                     // Przypisanie klienta HTTP.
            _configuration = configuration;               // Przypisanie konfiguracji.
        }

        public async Task<string> TranslateAsync(string text, string from, string to)
        {
            if (string.IsNullOrWhiteSpace(text))          // Jeśli tekst pusty...
                return string.Empty;                      // ...zwróć pusty string.

            if (from == to)                               // Jeśli języki są takie same...
                return text;                              // ...nie tłumacz, zwróć oryginał.

            var endpoint = _configuration["AzureTranslator:Endpoint"]!;  // Pobranie URL API.
            var key = _configuration["AzureTranslator:Key"]!;            // Pobranie klucza API.
            var region = _configuration["AzureTranslator:Region"]!;      // Pobranie regionu Azure.

            var url = $"{endpoint}translate?api-version=3.0&from={from}&to={to}"; // Budowa URL zapytania.

            var body = new[] { new { Text = text } };     // Tworzenie JSON z tekstem do tłumaczenia.

            var request = new HttpRequestMessage(HttpMethod.Post, url);  // Tworzenie żądania POST.
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);       // Dodanie klucza API.
            request.Headers.Add("Ocp-Apim-Subscription-Region", region); // Dodanie regionu API.
            request.Content = new StringContent(                           // Ustawienie treści żądania.
                JsonSerializer.Serialize(body),                           // Serializacja JSON.
                Encoding.UTF8,                                            // Kodowanie UTF‑8.
                "application/json");                                      // Typ treści JSON.

            var response = await _httpClient.SendAsync(request);          // Wysłanie żądania do Azure.
            response.EnsureSuccessStatusCode();                           // Rzuca wyjątek, jeśli status ≠ 200.

            var json = await response.Content.ReadAsStringAsync();        // Odczyt odpowiedzi jako string JSON.
            using var doc = JsonDocument.Parse(json);                     // Parsowanie JSON do dokumentu.

            var translatedText = doc.RootElement[0]                       // Pobranie pierwszego elementu tablicy.
                .GetProperty("translations")[0]                           // Pobranie pierwszego tłumaczenia.
                .GetProperty("text")                                      // Pobranie pola "text".
                .GetString();                                             // Odczyt wartości jako string.

            return translatedText ?? string.Empty;                        // Zwrócenie tłumaczenia lub pustego stringa.
        }
    }
}
