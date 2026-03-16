using Flight.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flight.Infrastructure.Database;

/// <summary>
/// Contexte principal Entity Framework Core de l'application FlightNet.
/// Cette classe représente la session de travail avec la base de données.
/// 
/// Chaque propriété DbSet correspond à une table en base.
/// Grâce à ce contexte, Entity Framework peut :
/// - lire les données
/// - insérer des données
/// - mettre à jour des données
/// - supprimer des données
/// </summary>
public class FlightContext : DbContext
{
    /// <summary>
    /// Initialise une nouvelle instance du contexte de base de données.
    /// </summary>
    /// <param name="options">Options de configuration du DbContext.</param>
    public FlightContext(DbContextOptions<FlightContext> options)
        : base(options)
    {
    }

    // ============================================================
    // Tables historiques / cœur métier existant
    // ============================================================

    /// <summary>
    /// Table des compagnies aériennes.
    /// </summary>
    public DbSet<Airline> Airlines { get; set; } = null!;

    /// <summary>
    /// Table des aéroports.
    /// </summary>
    public DbSet<Airport> Airports { get; set; } = null!;

    /// <summary>
    /// Table des réservations.
    /// </summary>
    public DbSet<Booking> Bookings { get; set; } = null!;

    /// <summary>
    /// Table des villes.
    /// </summary>
    public DbSet<City> Cities { get; set; } = null!;

    /// <summary>
    /// Table des pays.
    /// </summary>
    public DbSet<Country> Countries { get; set; } = null!;

    /// <summary>
    /// Table des vols.
    /// </summary>
    public DbSet<Domain.Entities.Flight> Flights { get; set; } = null!;

    /// <summary>
    /// Table des passagers.
    /// </summary>
    public DbSet<Passenger> Passengers { get; set; } = null!;

    /// <summary>
    /// Table des véhicules.
    /// </summary>
    public DbSet<Vehicle> Vehicles { get; set; } = null!;

    // ============================================================
    // Nouvelles tables multiutilisateurs / sécurité / équipe
    // ============================================================

    /// <summary>
    /// Table des utilisateurs de l'application.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Table des rôles.
    /// </summary>
    public DbSet<Role> Roles { get; set; } = null!;

    /// <summary>
    /// Table de liaison entre utilisateurs et rôles.
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; } = null!;

    /// <summary>
    /// Table des membres d'équipe / équipage.
    /// </summary>
    public DbSet<CrewMember> CrewMembers { get; set; } = null!;

    /// <summary>
    /// Table des avions de la flotte.
    /// </summary>
    public DbSet<Aircraft> Aircrafts { get; set; } = null!;

    // ============================================================
    // Nouvelles tables métier complémentaires
    // ============================================================

    /// <summary>
    /// Table des paiements.
    /// </summary>
    public DbSet<Payment> Payments { get; set; } = null!;

    /// <summary>
    /// Table des billets.
    /// </summary>
    public DbSet<Ticket> Tickets { get; set; } = null!;

    /// <summary>
    /// Table des attributions de sièges.
    /// </summary>
    public DbSet<SeatAssignment> SeatAssignments { get; set; } = null!;

    /// <summary>
    /// Table des bagages.
    /// </summary>
    public DbSet<Baggage> Baggages { get; set; } = null!;

    /// <summary>
    /// Table des notifications.
    /// </summary>
    public DbSet<Notification> Notifications { get; set; } = null!;

    /// <summary>
    /// Table des tâches internes.
    /// </summary>
    public DbSet<TaskItem> TaskItems { get; set; } = null!;

    /// <summary>
    /// Table des refresh tokens.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    /// <summary>
    /// Table des journaux d'audit.
    /// </summary>
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    // ============================================================
    // Configuration avancée du modèle
    // ============================================================

    /// <summary>
    /// Configure le modèle EF Core au moment de la création du mapping.
    /// C'est ici qu'on peut définir :
    /// - les clés
    /// - les index
    /// - les relations
    /// - les contraintes supplémentaires
    /// </summary>
    /// <param name="modelBuilder">Constructeur du modèle EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --------------------------------------------------------
        // Index utiles pour accélérer certaines recherches
        // --------------------------------------------------------

        modelBuilder.Entity<User>()
            .HasIndex(x => x.UserName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(x => x.TicketNumber)
            .IsUnique();

        modelBuilder.Entity<Booking>()
            .HasIndex(x => x.BookingReference)
            .IsUnique();

        modelBuilder.Entity<Aircraft>()
            .HasIndex(x => x.RegistrationNumber)
            .IsUnique();

        // --------------------------------------------------------
        // Relations UserRole
        // --------------------------------------------------------

        modelBuilder.Entity<UserRole>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // --------------------------------------------------------
        // Relations CrewMember
        // --------------------------------------------------------

        modelBuilder.Entity<CrewMember>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // --------------------------------------------------------
        // Relations Payment
        // --------------------------------------------------------

        modelBuilder.Entity<Payment>()
            .HasOne<Booking>()
            .WithMany()
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // --------------------------------------------------------
        // Relations Ticket
        // --------------------------------------------------------

        modelBuilder.Entity<Ticket>()
            .HasOne<Booking>()
            .WithMany()
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ticket>()
            .HasOne<Passenger>()
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        // --------------------------------------------------------
        // Relations SeatAssignment
        // --------------------------------------------------------

        modelBuilder.Entity<SeatAssignment>()
            .HasOne<Domain.Entities.Flight>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SeatAssignment>()
            .HasOne<Passenger>()
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        // --------------------------------------------------------
        // Relations Notification
        // --------------------------------------------------------

        modelBuilder.Entity<Notification>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // --------------------------------------------------------
        // Relations TaskItem
        // --------------------------------------------------------

        modelBuilder.Entity<TaskItem>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaskItem>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // --------------------------------------------------------
        // Relations RefreshToken
        // --------------------------------------------------------

        modelBuilder.Entity<RefreshToken>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // --------------------------------------------------------
        // Relations Baggage
        // --------------------------------------------------------

        modelBuilder.Entity<Baggage>()
            .HasOne<Booking>()
            .WithMany()
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Baggage>()
            .HasOne<Passenger>()
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Baggage>()
            .HasOne<Domain.Entities.Flight>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        // --------------------------------------------------------
        // Relations Flight -> Aircraft
        // --------------------------------------------------------

        modelBuilder.Entity<Domain.Entities.Flight>()
            .HasOne<Aircraft>()
            .WithMany()
            .HasForeignKey(x => x.AircraftId)
            .OnDelete(DeleteBehavior.SetNull);

        // --------------------------------------------------------
        // Valeurs décimales / précision utile pour montants
        // --------------------------------------------------------

        modelBuilder.Entity<Payment>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Baggage>()
            .Property(x => x.Weight)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Booking>()
            .Property(x => x.TotalAmount)
            .HasPrecision(18, 2);
    }
}
