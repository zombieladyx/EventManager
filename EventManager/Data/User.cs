using System;
using System.Collections.Generic;

namespace EventManager.Data;

public partial class User
{
    public string EMAIL { get; set; } = null!;

    public string PASSWORD { get; set; } = null!;

    public string ROLE { get; set; } = null!;

    public DateTime? LOGIN_TIME { get; set; }

    public int? FAILED_ATTEMPTS { get; set; }

    public string? ACCOUNT_LOCKED { get; set; } = "FALSE";

    public string? TERMS_ACCEPTED { get; set; } = "NO";
    public ICollection<User_Event> EventUsers { get; set; } = new List<User_Event>();
}
