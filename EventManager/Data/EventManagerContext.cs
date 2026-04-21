using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data;

public partial class EventManagerContext : DbContext
{
    //konstruktor bez parametrów
    public EventManagerContext()
    {
    }

    //konstruktor z opcjami
    public EventManagerContext(DbContextOptions<EventManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } //każdy rekord z tabeli users mapuje się na obiekt User
    public DbSet<Event> Events { get; set; } //każdy obiekt z tabeli Events mapuje się na obiekt Event

    public DbSet<User_Event> User_Event { get; set; } //każdy obiekt z tabeli EventUsers mapuje się na obiekt EventUser

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //konfiguracja encji Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__users__161CF725C0D8FC4C");
            //nazwa tabeli users
            entity.ToTable("users");
            //kolumna EMAIL
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            //kolumna PASSWORD
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            //kolumna ROLE
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLE");
        });

        //konfiguracja encji Event - dodano, aby EF Core miał zdefiniowany klucz główny
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EVENT_ID);
            entity.ToTable("Events");
        });

        //konfiguracja encji User_Event - klucz złożony z Email i EventId, relacje do User i Event
        modelBuilder.Entity<User_Event>()
            .HasKey(eu => new { eu.Email, eu.EVENT_ID });

        modelBuilder.Entity<User_Event>()
            .HasOne(eu => eu.User)  
            .WithMany(u => u.EventUsers)
            .HasForeignKey(eu => eu.Email);

        modelBuilder.Entity<User_Event>()
            .HasOne(eu => eu.Event)
            .WithMany(e => e.EventUsers)
            .HasForeignKey(eu => eu.EVENT_ID);

    OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
