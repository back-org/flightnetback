using Flight.Domain.Entities;
using Flight.Infrastructure.Implementations;

namespace Flight.Infrastructure.Interfaces;

/// <summary>
/// The airline service.
/// </summary>
internal sealed class AirlineService(IRepositoryManager repository, ILoggerManager logger)
    : Service<Airline>(repository, logger);

/// <summary>
/// The airport service.
/// </summary>
internal sealed class AirportService(IRepositoryManager repository, ILoggerManager logger)
    : Service<Airport>(repository, logger);

/// <summary>
/// The booking service.
/// </summary>
internal sealed class BookingService(IRepositoryManager repository, ILoggerManager logger)
    : Service<Booking>(repository, logger);

/// <summary>
/// The city service.
/// </summary>
internal sealed class CityService(IRepositoryManager repository, ILoggerManager logger)
    : Service<City>(repository, logger);

/// <summary>
/// The country service.
/// </summary>
internal sealed class CountryService(IRepositoryManager repository, ILoggerManager logger)
    : Service<Country>(repository, logger);

/// <summary>
/// The flight service.
/// </summary>
internal sealed class FlightService(IRepositoryManager repository, ILoggerManager logger)
    : Service<Domain.Entities.Flight>(repository, logger);

/// <summary>
/// The passenger service.
/// </summary>
internal sealed class PassengerService(IRepositoryManager repository, ILoggerManager logger)
    : Service<Passenger>(repository, logger);

/// <summary>
/// The vehicle service.
/// </summary>
internal sealed class VehicleService(IRepositoryManager repository, ILoggerManager logger)
    : Service<Vehicle>(repository, logger);

/// <summary>
/// The service manager.
/// </summary>
public interface IServiceManager
{
    /// <summary>
    /// Gets the airline service.
    /// </summary>
    IService<Airline> AirlineService { get; }

    /// <summary>
    /// Gets the airport service.
    /// </summary>
    IService<Airport> AirportService { get; }

    /// <summary>
    /// Gets the booking service.
    /// </summary>
    IService<Booking> BookingService { get; }

    /// <summary>
    /// Gets the city service.
    /// </summary>
    IService<City> CityService { get; }

    /// <summary>
    /// Gets the country service.
    /// </summary>
    IService<Country> CountryService { get; }

    /// <summary>
    /// Gets the flight service.
    /// </summary>
    IService<Domain.Entities.Flight> FlightService { get; }

    /// <summary>
    /// Gets the passenger service.
    /// </summary>
    IService<Passenger> PassengerService { get; }

    /// <summary>
    /// Gets the vehicle service.
    /// </summary>
    IService<Vehicle> VehicleService { get; }
}