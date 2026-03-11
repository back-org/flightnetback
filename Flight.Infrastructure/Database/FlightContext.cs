using Flight.Domain.Entities;
using Flight.Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flight.Infrastructure.Database;

/// <summary>
/// Contexte principal Entity Framework Core de l'application FlightNet.
/// </summary>
public class FlightContext : IdentityDbContext
{
    /// <summary>
    /// Initialise une nouvelle instance du contexte.
    /// </summary>
    /// <param name="options">Les options de configuration EF Core.</param>
    public FlightContext(DbContextOptions<FlightContext> options) : base(options)
    {
    }

    /// <summary>
    /// Table des compagnies aériennes.
    /// </summary>
    public DbSet<Airline> Airline { get; set; } = null!;

    /// <summary>
    /// Table des aéroports.
    /// </summary>
    public DbSet<Airport> Airport { get; set; } = null!;

    /// <summary>
    /// Table des vols.
    /// </summary>
    public DbSet<Domain.Entities.Flight> Flight { get; set; } = null!;

    /// <summary>
    /// Table des passagers.
    /// </summary>
    public DbSet<Passenger> Passenger { get; set; } = null!;

    /// <summary>
    /// Table des réservations.
    /// </summary>
    public DbSet<Booking> Booking { get; set; } = null!;

    /// <summary>
    /// Table des véhicules.
    /// </summary>
    public DbSet<Vehicle> Vehicle { get; set; } = null!;

    /// <summary>
    /// Table des pays.
    /// </summary>
    public DbSet<Country> Country { get; set; } = null!;

    /// <summary>
    /// Table des villes.
    /// </summary>
    public DbSet<City> City { get; set; } = null!;

    /// <summary>
    /// Configure le modèle EF Core.
    /// </summary>
    /// <param name="modelBuilder">Le builder EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CountryConfiguration());

        modelBuilder.Entity<Domain.Entities.Flight>()
            .HasIndex(f => f.Code)
            .IsUnique();

        modelBuilder.Entity<Domain.Entities.Flight>()
            .Property(f => f.Code)
            .HasMaxLength(30)
            .IsRequired();
    }
}