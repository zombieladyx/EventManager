using System;
using System.Collections.Generic;

namespace EventManager.Data;

public class User_Event
{
    // Email użytkownika będący kluczem obcym do tabeli User
    public required string EMAIL { get; set; }

    // Id eventu będące kluczem obcym do tabeli Event
    public required string EVENT_ID { get; set; }

    // Nawigacja do obiektu użytkownika powiązanego z tym wpisem
    public User User { get; set; } = null!;

    // Nawigacja do obiektu eventu powiązanego z tym wpisem
    public Event Event { get; set; } = null!;
}
