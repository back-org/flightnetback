using Flight.Domain.Core.Abstracts;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Contracts;
using Flight.Infrastructure.Database;

namespace Flight.Infrastructure.Interfaces;

/// <summary>
/// The airline repository.
/// </summary>
public class AirlineRepository(FlightContext context) : GenericRepository<Airline>(context);

/// <summary>
/// The airport repository.
/// </summary>
public class AirportRepository(FlightContext context) : GenericRepository<Airport>(context);

/// <summary>
/// The booking repository.
/// </summary>
public class BookingRepository(FlightContext context) : GenericRepository<Booking>(context);

/// <summary>
/// The city repository.
/// </summary>
public class CityRepository(FlightContext context) : GenericRepository<City>(context);

/// <summary>
/// The country repository.
/// </summary>
public class CountryRepository(FlightContext context) : GenericRepository<Country>(context);

/// <summary>
/// The flight repository.
/// </summary>
public class FlightRepository(FlightContext context) : GenericRepository<Domain.Entities.Flight>(context);

/// <summary>
/// The passenger repository.
/// </summary>
public class PassengerRepository(FlightContext context) : GenericRepository<Passenger>(context);

/// <summary>
/// The vehicle repository.
/// </summary>
public class VehicleRepository(FlightContext context) : GenericRepository<Vehicle>(context);

/// <summary>
/// The repository manager.
/// </summary>
public interface IRepositoryManager
{
    IGenericRepository<DeleteEntity<int>> Entity { get; }

    /// <summary>
    /// Gets the airline.
    /// </summary>
    IGenericRepository<Airline> Airline { get; }

    /// <summary>
    /// Gets the airport.
    /// </summary>
    IGenericRepository<Airport> Airport { get; }

    /// <summary>
    /// Gets the booking.
    /// </summary>
    IGenericRepository<Booking> Booking { get; }

    /// <summary>
    /// Gets the city.
    /// </summary>
    IGenericRepository<City> City { get; }

    /// <summary>
    /// Gets the country.
    /// </summary>
    IGenericRepository<Country> Country { get; }

    /// <summary>
    /// Gets the flight.
    /// </summary>
    IGenericRepository<Domain.Entities.Flight> Flight { get; }

    /// <summary>
    /// Gets the passenger.
    /// </summary>
    IGenericRepository<Passenger> Passenger { get; }

    /// <summary>
    /// Gets the vehicle.
    /// </summary>
    IGenericRepository<Vehicle> Vehicle { get; }

    /// <summary>
    /// Saves the.
    /// </summary>
    void Save();
}