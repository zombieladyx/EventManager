using EventManager.Data;

namespace EventManager.Components.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        public LanguageMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var languageState = context.RequestServices
                                       .GetRequiredService<LanguageStateService>();
            var lang = context.Request.Cookies["selectedLanguage"] ?? "en";
            languageState.InitializeLanguage(lang);
            await _next(context);
        }
    }
}