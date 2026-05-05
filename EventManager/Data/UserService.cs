using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
namespace EventManager.Data
{
    //serwis odpowiedzialny za operacje na użytkownikach
    public class UserService
    {
        //DbContext (Entity Framework Core), który daje dostęp do bazy danych
        private readonly EventManagerContext context;
        // Umożliwia dostęp do aktualnego żądania HTTP oraz danych zalogowanego użytkownika
        private readonly IHttpContextAccessor _httpContextAccessor;

        //konstruktor, który wstrzykuje DbContext przez DI, pozwala wykonywać zapytania do bazy
        public UserService(EventManagerContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //rejestracja użytkowników
        public bool SaveUser(User user)
        {
            //sprawdzenie, czy istnieje użytkownik o takim samym emailu
            bool isExist = context.Users.Any(x => x.EMAIL == user.EMAIL);
            //jeśli nie istnieje: dodanie użytkownika do tabeli Users i zapisanie zmian do bazy
            if (!isExist)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            //jeśli istnieje: zwróć false
            return false;
        }

        //logowanie użytkownika, szukanie użytkownika o podanym emailu i haśle
        public User? Verify(string email, string password)
        {
            //zaszyfrowania hasła podanego przez użytkownika przy logowaniu
            string hashPassword;
            hashPassword = EncryptProvider.Sha256(password);

            //zwróci użytkownika, jeśli dane są poprawne albo null jeśli nie ma takiego użytkownika
            //porównanie emaila ignoruje wielkość liter, porównanie hasła jest bezpośrednie
            return context.Users.FirstOrDefault(x => x.EMAIL.ToLower() == email.ToLower()
                    && x.PASSWORD == hashPassword);
        }


        // Zwraca email aktualnie zalogowanego użytkownika
        public string GetEmail()
        {
            return _httpContextAccessor.HttpContext?.User
                // Najpierw próbuje pobrać email z claimu Email
                ?.FindFirstValue(ClaimTypes.Email)
                // Jeśli email nie istnieje, bierze nazwę użytkownika z claimu Name
                ?? _httpContextAccessor.HttpContext?.User?.Identity?.Name
                // Jeśli nic nie znaleziono, zwraca pusty string
                ?? string.Empty;
        }
        //funkcje związane z regulaminem
        // Aktualizuje pole TERMS_ACCEPTED w bazie danych.
        public async Task UpdateTermsAcceptanceAsync(string email, string acceptanceValue)
        {
            var user = await context.Users.FindAsync(email);

            // Jeśli użytkownik nie istnieje, przerywamy operację.
            if (user == null)
                return;

            // Zapisujemy wartość akceptacji regulaminu, np. "yes" albo "no".
            user.TERMS_ACCEPTED = acceptanceValue;

            await context.SaveChangesAsync();
        }
        // Pobiera wyłącznie wartość pola TERMS_ACCEPTED bez ładowania całego encji.
        public async Task<string?> GetTermsAcceptanceAsync(string email)
        {
            return await context.Users
                .Where(x => x.EMAIL == email)
                .Select(x => x.TERMS_ACCEPTED)
                .FirstOrDefaultAsync();
        }

        //Wywolanie procedury Login_Failed na serwerze SQL
        public async Task HandleFailedLogin(string email)
        {
            await context.Database.ExecuteSqlAsync($"EXEC dbo.Login_Failed @Email = {email}");
        }

        //Wywolanie procedury Login_Success na serwerze SQL
        public async Task HandleSuccessLogin(string email)
        {
            await context.Database.ExecuteSqlAsync($"EXEC dbo.Login_Success @Email = {email}");
        }
    }
}