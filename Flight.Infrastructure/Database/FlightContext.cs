using Flight.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Flight.Infrastructure.EntityConfigurations;

namespace Flight.Infrastructure.Database;

/// <summary>
/// Contexte Entity Framework Core de l'application FlightNet.
/// Hérite de <see cref="IdentityDbContext"/> pour intégrer ASP.NET Core Identity.
/// Déclare tous les <see cref="DbSet{TEntity}"/> utilisés dans l'application.
/// </summary>
public class FlightContext : IdentityDbContext
{
    /// <summary>
    /// Table des compagnies aériennes.
    /// </summary>
    public DbSet<Airline> Airline { get; set; }

    /// <summary>
    /// Table des aéroports.
    /// </summary>
    public DbSet<Airport> Airport { get; set; }

    /// <summary>
    /// Table des vols.
    /// </summary>
    public DbSet<Domain.Entities.Flight> Flight { get; set; }

    /// <summary>
    /// Table des passagers.
    /// </summary>
    public DbSet<Passenger> Passenger { get; set; }

    /// <summary>
    /// Table des réservations.
    /// </summary>
    public DbSet<Booking> Booking { get; init; }

    /// <summary>
    /// Table des véhicules de transport.
    /// </summary>
    public DbSet<Vehicle> Vehicle { get; set; }

    /// <summary>
    /// Table des pays.
    /// </summary>
    public DbSet<Country> Country { get; set; }

    /// <summary>
    /// Table des villes.
    /// </summary>
    public DbSet<City> City { get; set; }

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="FlightContext"/>.
    /// Les options de connexion sont injectées par l'infrastructure DI (voir <c>DatabaseMiddleware</c>).
    /// </summary>
    /// <param name="options">Options EF Core (chaîne de connexion, provider, etc.).</param>
    public FlightContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// Configure le modèle EF Core en appliquant les configurations Fluent API définies dans
    /// <c>Flight.Infrastructure.EntityConfigurations</c>.
    /// </summary>
    /// <param name="modelBuilder">Le constructeur de modèle EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
    }
}
