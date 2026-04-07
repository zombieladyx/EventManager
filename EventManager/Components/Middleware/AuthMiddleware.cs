using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace EventManager.Components.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next; //delegat, który przekazuje żądanie dalej, jeśli middleware go nie obsłuży

        //tymczasowy magazyn zalogowanych użytkowników, zawiera unikalny identyfikator sesji logowania i tożsamość użytkownika
        public static IDictionary<Guid, ClaimsPrincipal> Logins {  get; private set; } = new ConcurrentDictionary<Guid, ClaimsPrincipal>();

        //standardowy konstruktor middleware, zapisuje delegat next
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //sprawdzenie, czy to żądanie logowania
            if(context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
            {
                //pobranie wygenerowanego klucza
                var key = Guid.Parse(context.Request.Query["key"]);
                //pobranie tożsamości użytkownika
                var claim = Logins[key];
                //tworzenie cookie autoryzacyjnych, ustawienie użytkownika jako zalogowanego, Authorize View i spółka mogą działać
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claim);
                //przekierowanie na stronę główną
                context.Response.Redirect("/");
            }
            //jeśli to nie logowanie: przepuszczamy dalej, nie blokujemy innych żądań
            else
            {
                await next(context);
            }
        }
    }
}
