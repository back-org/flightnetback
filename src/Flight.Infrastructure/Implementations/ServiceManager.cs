using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using System;

namespace Flight.Infrastructure.Implementations;

/// <summary>
/// The service manager.
/// </summary>
public sealed class ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger) : IServiceManager
{
    private readonly Lazy<IService<Airline>> _airlineService = new(() => new AirlineService(repositoryManager, logger));
    private readonly Lazy<IService<Airport>> _airportService = new(() => new AirportService(repositoryManager, logger));
    private readonly Lazy<IService<Booking>> _bookingService = new(() => new BookingService(repositoryManager, logger));
    private readonly Lazy<IService<City>> _cityService = new(() => new CityService(repositoryManager, logger));
    private readonly Lazy<IService<Country>> _countryService = new(() => new CountryService(repositoryManager, logger));

    private readonly Lazy<IService<Domain.Entities.Flight>> _flightService =
        new(() => new FlightService(repositoryManager, logger));

    private readonly Lazy<IService<Passenger>> _passengerService =
        new(() => new PassengerService(repositoryManager, logger));

    private readonly Lazy<IService<Vehicle>> _vehicleService = new(() => new VehicleService(repositoryManager, logger));

    /// <summary>
    /// Gets the airline service.
    /// </summary>
    public IService<Airline> AirlineService => _airlineService.Value;

    /// <summary>
    /// Gets the airport service.
    /// </summary>
    public IService<Airport> AirportService => _airportService.Value;

    /// <summary>
    /// Gets the booking service.
    /// </summary>
    public IService<Booking> BookingService => _bookingService.Value;

    /// <summary>
    /// Gets the city service.
    /// </summary>
    public IService<City> CityService => _cityService.Value;

    /// <summary>
    /// Gets the country service.
    /// </summary>
    public IService<Country> CountryService => _countryService.Value;

    /// <summary>
    /// Gets the flight service.
    /// </summary>
    public IService<Domain.Entities.Flight> FlightService => _flightService.Value;

    /// <summary>
    /// Gets the passenger service.
    /// </summary>
    public IService<Passenger> PassengerService => _passengerService.Value;

    /// <summary>
    /// Gets the vehicle service.
    /// </summary>
    public IService<Vehicle> VehicleService => _vehicleService.Value;
}