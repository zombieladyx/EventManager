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
    //public DbSet<Event> Events { get; set; } //każdy obiekt z tabeli Events mapuje się na obiekt Event

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
