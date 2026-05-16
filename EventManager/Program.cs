//dostęp do komponentów Blazor
using EventManager.Components;
//własne middleware (warstwa oprogramowania do komunikacji pomiędzy różnymi aplikacjami)
using EventManager.Components.Middleware;
//przestrzeń nazw z EventManagerContext
using EventManager.Data;
using Microsoft.AspNetCore.Authentication;

//dla cookie-based auth
using Microsoft.AspNetCore.Authentication.Cookies;
//dla konfiguracji EF Core (EntityFramework Core)
using Microsoft.EntityFrameworkCore;

//utworzenie obiketu builder, który zbiera konfiguracje, pozwala rejestrować serwisy itp.
var builder = WebApplication.CreateBuilder(args);

//dla poprawnego działania komponentów .razor
builder.Services.AddRazorComponents() //rejestruje obsługę komponentów Razor (Blazor)
    .AddInteractiveServerComponents(); //włącza tryb interaktywny po stronie serwera (Blazor Server)

//konfiguracja bazy danych
builder.Services.AddDbContext<EventManagerContext>(options => //rejestracja kontekstu EF Core w Dependency Injection
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //wykorzystanie SQL Servera, pobranie connection string do bazy danych z appsettings.json
});

//rejestracja serwisu użytkowników
builder.Services.AddScoped<UserService>(); //rejestracja UserService jako serwis o czasie życia scoped (nowa instancja na każde żądanie HTTP)
builder.Services.AddScoped<IEventService, EventService>(); //rejestracja EventService

//konfiguracja uwierzytelniania (cookies)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //włącza mechanizm uwierzytelniania w aplikacji i używa schematu opartego na cookies
    .AddCookie(options => //konfiguracja cookie auth
    {
        options.LoginPath = "/login"; //jeśli użytkownik nie jest zalogowany, a wejdzie na stronę wymagającą autoryzacji, zostanie przekierowany na /login
        options.AccessDeniedPath = "/accessdenied"; //jeśli użytkownik jest zalogowany, ale nie ma uprawnień, zostanie przekierowany na /accessdenied
    });

//autoryzacja
builder.Services.AddAuthorization(); //włącza system autoryzacji


//rejestracja serwisów związanych z tłumaczeniami i językami
builder.Services.AddScoped<LanguageStateService>(); //rejestracja LanguageStateService, który będzie przechowywał aktualny język aplikacji i powiadamiał o zmianach
builder.Services.AddHttpClient<TranslatorService>(); //rejestracja TranslatorService, który będzie korzystał z HttpClient do komunikacji z zewnętrznym API tłumaczeń

//rejestracja serwisu do odczytania aktualnego użytkownika z HttpContext
builder.Services.AddHttpContextAccessor();// Rejestruje usługę IHttpContextAccessor, aby można było odczytywać aktualny HttpContext // i dane zalogowanego użytkownika, np. email z claims.
//budowanie aplikacji
var app = builder.Build();

//obłsuga błędów i HSTS (mechanizm bezpieczeństwa, który wymusza na przeglądarkach internetowych łączenie się z witryną wyłącznie przez szyfrowany protokół HTTPS)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true); //globalny handler wyjątków, przekierowuje na /Error
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); //włącza HSTS
}

//strony błędów i HTTPS
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true); //dla kodów typu 404, 500 itd. przekierowuje logicznie na /not-found
app.UseHttpsRedirection(); //przekierowuje z HTTP na HTTPS

//uwierzytelnianie, autoryzacja, middleware
app.UseAuthentication(); //odczytuje cookie
app.UseAuthorization(); //sprawdza, czy użytkownik ma dostęp
app.UseMiddleware<AuthMiddleware>(); //własne middleware
//app.UseMiddleware<LanguageMiddleware>(); //własne middleware do ustawiania języka na podstawie cookie

//antiforgery
app.UseAntiforgery(); //włącza ochronę przed CSRF
//wylogowanie użytkownika przez endpoint /logout, który wywołuje SignOutAsync, usuwając cookie uwierzytelniające, a następnie przekierowuje na stronę główną
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return Results.Redirect("/");
});
//statyczne pliki i mapowanie komponentów
app.MapStaticAssets(); //obsługa plików statycznych, np. css
app.MapRazorComponents<App>() //głównym komponentem aplikacji jest App
    .AddInteractiveServerRenderMode(); //włącza interaktywny tryb Blazor Server dla tych komponentów

//uruchomienie aplikacji
app.Run();
