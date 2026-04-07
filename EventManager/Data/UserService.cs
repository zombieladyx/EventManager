namespace EventManager.Data
{
    //serwis odpowiedzialny za operacje na użytkownikach
    public class UserService
    {
        //DbContext (Entity Framework Core), który daje dostęp do bazy danych
        private readonly EventManagerContext context;

        //konstruktor, który wstrzykuje DbContext przez DI, pozwala wykonywać zapytania do bazy
        public UserService(EventManagerContext context)
        {
            this.context = context;
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
            //zwróci użytkownika, jeśli dane są poprawne albo null jeśli nie ma takiego użytkownika
            //porównanie emaila ignoruje wielkość liter, porównanie hasła jest bezpośrednie
            return context.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower()
                    && x.Password == password);
        }
    }
}
