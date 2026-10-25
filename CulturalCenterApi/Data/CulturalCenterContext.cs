using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CulturalCenterApi.Models;

public partial class CulturalCenterContext : DbContext
{
    public CulturalCenterContext()
    {
    }

    public CulturalCenterContext(DbContextOptions<CulturalCenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<LogsAction> LogsActions { get; set; }

    public virtual DbSet<LogsError> LogsErrors { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TypesEvent> TypesEvents { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("Employees_pkey");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.NumberPhone).HasMaxLength(11);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.IdEvent).HasName("Events_pkey");

            entity.Property(e => e.EventDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EventName).HasMaxLength(150);
            entity.Property(e => e.IdTypeEvent).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdOrganizerNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.IdOrganizer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Events_IdOrganizer_fkey");

            entity.HasOne(d => d.IdTypeEventNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.IdTypeEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Events_IdTypeEvent_fkey");
        });

        modelBuilder.Entity<LogsAction>(entity =>
        {
            entity.HasKey(e => e.IdRecordLogAction).HasName("LogsActions_pkey");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.ActionDateTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ActionSystem).HasMaxLength(50);
            entity.Property(e => e.Object).HasMaxLength(100);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.LogsActions)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LogsActions_IdUser_fkey");
        });

        modelBuilder.Entity<LogsError>(entity =>
        {
            entity.HasKey(e => e.IdRecordLogError).HasName("LogsErrors_pkey");

            entity.Property(e => e.ErrorDateTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ErrorSystem).HasMaxLength(50);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.LogsErrors)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LogsErrors_IdUser_fkey");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.HasKey(e => e.IdOrganizer).HasName("Organizers_pkey");

            entity.Property(e => e.OrganizerEmail).HasMaxLength(100);
            entity.Property(e => e.OrganizerName).HasMaxLength(255);
            entity.Property(e => e.OrganizerNumberPhone).HasMaxLength(11);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("Tickets_pkey");

            entity.Property(e => e.Seat).HasMaxLength(10);
            entity.Property(e => e.SellDateTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UnicTicketNumber).HasMaxLength(10);

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tickets_IdEvent_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("Tickets_IdUser_fkey");
        });

        modelBuilder.Entity<TypesEvent>(entity =>
        {
            entity.HasKey(e => e.IdTypeEvent).HasName("TypesEvents_pkey");

            entity.Property(e => e.TypeEvent).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("Users_pkey");

            entity.HasIndex(e => e.IdEmployee, "Users_IdEmployee_fkey");

            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("Users_IdEmployee_fkey")
                .OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
