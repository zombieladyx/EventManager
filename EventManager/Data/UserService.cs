using System.Security.Claims;
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
            bool isExist = context.Users.Any(x => x.Email == user.Email);
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
            string encryptPassword;
            encryptPassword = Encrypt(password);

            //zwróci użytkownika, jeśli dane są poprawne albo null jeśli nie ma takiego użytkownika
            //porównanie emaila ignoruje wielkość liter, porównanie hasła jest bezpośrednie
            return context.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower()
                    && x.Password == encryptPassword);
        }

        //metoda szyfrująca
        public string Encrypt(string plainText)
        {
            string encryptPassword = Eramake.eCryptography.Encrypt(plainText);
            return encryptPassword;
        }

        //HTTP context do pobierania informacji o aktualnie zalogowanym użytkowniku
      

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
    }
}
