using Flight.Domain.Entities;
using Flight.Domain.Interfaces;

namespace Flight.Infrastructure.Interfaces;

/// <summary>
/// Représente le gestionnaire central des repositories.
/// Cette interface regroupe tous les accès aux dépôts de données de l'application.
/// 
/// Le but est d'avoir un seul point d'entrée pour accéder aux repositories,
/// au lieu d'injecter chaque repository séparément partout dans l'application.
/// </summary>
public interface IRepositoryManager
{
    // ============================================================
    // Repositories historiques / cœur métier
    // ============================================================

    /// <summary>
    /// Repository des compagnies aériennes.
    /// </summary>
    IGenericRepository<Airline> Airline { get; }

    /// <summary>
    /// Repository des aéroports.
    /// </summary>
    IGenericRepository<Airport> Airport { get; }

    /// <summary>
    /// Repository des réservations.
    /// </summary>
    IGenericRepository<Booking> Booking { get; }

    /// <summary>
    /// Repository des villes.
    /// </summary>
    IGenericRepository<City> City { get; }

    /// <summary>
    /// Repository des pays.
    /// </summary>
    IGenericRepository<Country> Country { get; }

    /// <summary>
    /// Repository des vols.
    /// </summary>
    IGenericRepository<Domain.Entities.Flight> Flight { get; }

    /// <summary>
    /// Repository des passagers.
    /// </summary>
    IGenericRepository<Passenger> Passenger { get; }

    /// <summary>
    /// Repository des véhicules.
    /// </summary>
    IGenericRepository<Vehicle> Vehicle { get; }

    // ============================================================
    // Repositories multiutilisateurs / sécurité
    // ============================================================

    /// <summary>
    /// Repository des utilisateurs.
    /// </summary>
    IGenericRepository<User> User { get; }

    /// <summary>
    /// Repository des rôles.
    /// </summary>
    IGenericRepository<Role> Role { get; }

    /// <summary>
    /// Repository des associations utilisateur-rôle.
    /// </summary>
    IGenericRepository<UserRole> UserRole { get; }

    /// <summary>
    /// Repository des refresh tokens.
    /// </summary>
    IGenericRepository<RefreshToken> RefreshToken { get; }

    // ============================================================
    // Repositories équipe / exploitation
    // ============================================================

    /// <summary>
    /// Repository des membres d'équipe.
    /// </summary>
    IGenericRepository<CrewMember> CrewMember { get; }

    /// <summary>
    /// Repository des avions.
    /// </summary>
    IGenericRepository<Aircraft> Aircraft { get; }

    // ============================================================
    // Repositories métier complémentaires
    // ============================================================

    /// <summary>
    /// Repository des paiements.
    /// </summary>
    IGenericRepository<Payment> Payment { get; }

    /// <summary>
    /// Repository des billets.
    /// </summary>
    IGenericRepository<Ticket> Ticket { get; }

    /// <summary>
    /// Repository des attributions de sièges.
    /// </summary>
    IGenericRepository<SeatAssignment> SeatAssignment { get; }

    /// <summary>
    /// Repository des notifications.
    /// </summary>
    IGenericRepository<Notification> Notification { get; }

    /// <summary>
    /// Repository des tâches internes.
    /// </summary>
    IGenericRepository<TaskItem> TaskItem { get; }

    /// <summary>
    /// Repository des bagages.
    /// </summary>
    IGenericRepository<Baggage> Baggage { get; }

    /// <summary>
    /// Repository des journaux d'audit.
    /// </summary>
    IGenericRepository<AuditLog> AuditLog { get; }
}
