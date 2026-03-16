/*
 * Rôle métier du fichier: Fournir les implémentations techniques des services et dépôts métier.
 * Description: Ce fichier participe au sous-domaine 'Flight.Infrastructure/Contracts' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Database;
using Flight.Infrastructure.Interfaces;

namespace Flight.Infrastructure.Contracts;

/// <summary>
/// Implémentation concrète du gestionnaire central des repositories.
/// Cette classe instancie les repositories à la demande et les réutilise ensuite.
/// 
/// Cela permet :
/// - d'éviter la duplication
/// - de centraliser l'accès aux données
/// - de simplifier l'injection de dépendances
/// </summary>
public class RepositoryManager : IRepositoryManager
{
    private readonly FlightContext _context;

    // ============================================================
    // Champs privés pour stocker les repositories déjà créés
    // ============================================================

    private IGenericRepository<Airline>? _airline;
    private IGenericRepository<Airport>? _airport;
    private IGenericRepository<Booking>? _booking;
    private IGenericRepository<City>? _city;
    private IGenericRepository<Country>? _country;
    private IGenericRepository<Domain.Entities.Flight>? _flight;
    private IGenericRepository<Passenger>? _passenger;
    private IGenericRepository<Vehicle>? _vehicle;

    private IGenericRepository<User>? _user;
    private IGenericRepository<Role>? _role;
    private IGenericRepository<UserRole>? _userRole;
    private IGenericRepository<RefreshToken>? _refreshToken;

    private IGenericRepository<CrewMember>? _crewMember;
    private IGenericRepository<Aircraft>? _aircraft;

    private IGenericRepository<Payment>? _payment;
    private IGenericRepository<Ticket>? _ticket;
    private IGenericRepository<SeatAssignment>? _seatAssignment;
    private IGenericRepository<Notification>? _notification;
    private IGenericRepository<TaskItem>? _taskItem;
    private IGenericRepository<Baggage>? _baggage;
    private IGenericRepository<AuditLog>? _auditLog;

    /// <summary>
    /// Initialise une nouvelle instance du gestionnaire de repositories.
    /// </summary>
    /// <param name="context">Contexte EF Core utilisé pour accéder à la base de données.</param>
    public RepositoryManager(FlightContext context)
    {
        _context = context;
    }

    // ============================================================
    // Repositories historiques / cœur métier
    // ============================================================

    /// <inheritdoc />
    public IGenericRepository<Airline> Airline =>
        _airline ??= new GenericRepository<Airline>(_context);

    /// <inheritdoc />
    public IGenericRepository<Airport> Airport =>
        _airport ??= new GenericRepository<Airport>(_context);

    /// <inheritdoc />
    public IGenericRepository<Booking> Booking =>
        _booking ??= new GenericRepository<Booking>(_context);

    /// <inheritdoc />
    public IGenericRepository<City> City =>
        _city ??= new GenericRepository<City>(_context);

    /// <inheritdoc />
    public IGenericRepository<Country> Country =>
        _country ??= new GenericRepository<Country>(_context);

    /// <inheritdoc />
    public IGenericRepository<Domain.Entities.Flight> Flight =>
        _flight ??= new GenericRepository<Domain.Entities.Flight>(_context);

    /// <inheritdoc />
    public IGenericRepository<Passenger> Passenger =>
        _passenger ??= new GenericRepository<Passenger>(_context);

    /// <inheritdoc />
    public IGenericRepository<Vehicle> Vehicle =>
        _vehicle ??= new GenericRepository<Vehicle>(_context);

    // ============================================================
    // Repositories multiutilisateurs / sécurité
    // ============================================================

    /// <inheritdoc />
    public IGenericRepository<User> User =>
        _user ??= new GenericRepository<User>(_context);

    /// <inheritdoc />
    public IGenericRepository<Role> Role =>
        _role ??= new GenericRepository<Role>(_context);

    /// <inheritdoc />
    public IGenericRepository<UserRole> UserRole =>
        _userRole ??= new GenericRepository<UserRole>(_context);

    /// <inheritdoc />
    public IGenericRepository<RefreshToken> RefreshToken =>
        _refreshToken ??= new GenericRepository<RefreshToken>(_context);

    // ============================================================
    // Repositories équipe / exploitation
    // ============================================================

    /// <inheritdoc />
    public IGenericRepository<CrewMember> CrewMember =>
        _crewMember ??= new GenericRepository<CrewMember>(_context);

    /// <inheritdoc />
    public IGenericRepository<Aircraft> Aircraft =>
        _aircraft ??= new GenericRepository<Aircraft>(_context);

    // ============================================================
    // Repositories métier complémentaires
    // ============================================================

    /// <inheritdoc />
    public IGenericRepository<Payment> Payment =>
        _payment ??= new GenericRepository<Payment>(_context);

    /// <inheritdoc />
    public IGenericRepository<Ticket> Ticket =>
        _ticket ??= new GenericRepository<Ticket>(_context);

    /// <inheritdoc />
    public IGenericRepository<SeatAssignment> SeatAssignment =>
        _seatAssignment ??= new GenericRepository<SeatAssignment>(_context);

    /// <inheritdoc />
    public IGenericRepository<Notification> Notification =>
        _notification ??= new GenericRepository<Notification>(_context);

    /// <inheritdoc />
    public IGenericRepository<TaskItem> TaskItem =>
        _taskItem ??= new GenericRepository<TaskItem>(_context);

    /// <inheritdoc />
    public IGenericRepository<Baggage> Baggage =>
        _baggage ??= new GenericRepository<Baggage>(_context);

    /// <inheritdoc />
    public IGenericRepository<AuditLog> AuditLog =>
        _auditLog ??= new GenericRepository<AuditLog>(_context);
}
