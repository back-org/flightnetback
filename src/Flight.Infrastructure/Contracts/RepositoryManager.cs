using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using System;
using Flight.Domain.Core.Abstracts;
using Flight.Infrastructure.Database;

namespace Flight.Infrastructure.Contracts;

/// <summary>
/// The repository manager.
/// </summary>
public sealed class RepositoryManager(FlightContext repositoryContext) : IRepositoryManager
{
    private readonly Lazy<IGenericRepository<Airline>> _airlineRepository = new(() => new
        AirlineRepository(repositoryContext));

    private readonly Lazy<IGenericRepository<Airport>> _airportRepository = new(() => new
        AirportRepository(repositoryContext));

    private readonly Lazy<IGenericRepository<Booking>> _bookingRepository = new(() => new
        BookingRepository(repositoryContext));

    private readonly Lazy<IGenericRepository<City>> _cityRepository = new(() => new
        CityRepository(repositoryContext));

    private readonly Lazy<IGenericRepository<Country>> _countryRepository = new(() => new
        CountryRepository(repositoryContext));

    private readonly Lazy<IGenericRepository<Domain.Entities.Flight>> _flightRepository = new(() => new
        FlightRepository(repositoryContext));

    private readonly Lazy<IGenericRepository<Passenger>> _passengerRepository = new(() => new
        PassengerRepository(repositoryContext));

    private readonly Lazy<IGenericRepository<Vehicle>> _vehicleRepository = new(() => new
        VehicleRepository(repositoryContext));

    public IGenericRepository<DeleteEntity<int>> Entity { get; }

    /// <summary>
    /// Gets the airline.
    /// </summary>
    public IGenericRepository<Airline> Airline => _airlineRepository.Value;

    /// <summary>
    /// Gets the airport.
    /// </summary>
    public IGenericRepository<Airport> Airport => _airportRepository.Value;

    /// <summary>
    /// Gets the booking.
    /// </summary>
    public IGenericRepository<Booking> Booking => _bookingRepository.Value;

    /// <summary>
    /// Gets the city.
    /// </summary>
    public IGenericRepository<City> City => _cityRepository.Value;

    /// <summary>
    /// Gets the country.
    /// </summary>
    public IGenericRepository<Country> Country => _countryRepository.Value;

    /// <summary>
    /// Gets the flight.
    /// </summary>
    public IGenericRepository<Domain.Entities.Flight> Flight => _flightRepository.Value;

    /// <summary>
    /// Gets the passenger.
    /// </summary>
    public IGenericRepository<Passenger> Passenger => _passengerRepository.Value;

    /// <summary>
    /// Gets the vehicle.
    /// </summary>
    public IGenericRepository<Vehicle> Vehicle => _vehicleRepository.Value;

    /// <summary>
    /// Saves the.
    /// </summary>
    public void Save()
    {
        repositoryContext.SaveChanges();
    }
}