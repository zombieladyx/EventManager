using System;
using System.Collections.Generic;

namespace EventManager.Data;

//encja EF Core, czyli klasa reprezentująca jeden rekord w tabeli users
//każda właściwość odpowiada jednej kolumnie w tabeli users
public partial class User
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
    public string TERMS_ACCEPTED { get; set; } = "NO";
    public ICollection<User_Event> EventUsers { get; set; } = new List<User_Event>();
}