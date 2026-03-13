using Flight.Domain.Entities;

namespace Flight.Application.DTOs;

/// <summary>
/// Contient les méthodes d'extension permettant la conversion
/// entre les entités métier du domaine et les objets de transfert de données (DTO).
/// </summary>
public static class DomainExtensions
{
    #region Airline

    /// <summary>
    /// Convertit une entité <see cref="Airline"/> en <see cref="AirlineDto"/>.
    /// </summary>
    /// <param name="entity">Entité compagnie aérienne à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static AirlineDto ToDto(this Airline entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AirlineDto(
            entity.Id,
            entity.Name,
            entity.State,
            entity.DeletedFlag
        );
    }

    /// <summary>
    /// Convertit un <see cref="AirlineDto"/> en entité <see cref="Airline"/>.
    /// </summary>
    /// <param name="dto">DTO compagnie aérienne à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Airline ToEntity(this AirlineDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Airline(
            dto.Id,
            dto.Name,
            dto.State,
            dto.DeletedFlag
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="Airline"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Airline entity, AirlineDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.State = dto.State;
        entity.DeletedFlag = dto.DeletedFlag;
    }

    #endregion

    #region Airport

    /// <summary>
    /// Convertit une entité <see cref="Airport"/> en <see cref="AirportDto"/>.
    /// </summary>
    /// <param name="entity">Entité aéroport à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static AirportDto ToDto(this Airport entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AirportDto(
            entity.Id,
            entity.Name,
            entity.CityId,
            entity.State,
            entity.DeletedFlag
        );
    }

    /// <summary>
    /// Convertit un <see cref="AirportDto"/> en entité <see cref="Airport"/>.
    /// </summary>
    /// <param name="dto">DTO aéroport à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Airport ToEntity(this AirportDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Airport(
            dto.Id,
            dto.Name,
            dto.CityId,
            dto.State,
            dto.DeletedFlag
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="Airport"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Airport entity, AirportDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.CityId = dto.CityId;
        entity.State = dto.State;
        entity.DeletedFlag = dto.DeletedFlag;
    }

    #endregion

    #region Booking

    /// <summary>
    /// Convertit une entité <see cref="Booking"/> en <see cref="BookingDto"/>.
    /// </summary>
    /// <param name="entity">Entité réservation à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static BookingDto ToDto(this Booking entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new BookingDto(
            entity.Id,
            entity.FlightType,
            entity.FlightId,
            entity.PassengerId,
            entity.Statut
        );
    }

    /// <summary>
    /// Convertit un <see cref="BookingDto"/> en entité <see cref="Booking"/>.
    /// </summary>
    /// <param name="dto">DTO réservation à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Booking ToEntity(this BookingDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Booking(
            dto.Id,
            dto.FlightType,
            dto.FlightId,
            dto.PassengerId,
            dto.Statut
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="Booking"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Booking entity, BookingDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.FlightType = dto.FlightType;
        entity.FlightId = dto.FlightId;
        entity.PassengerId = dto.PassengerId;
        entity.Statut = dto.Statut;
    }

    #endregion

    #region City

    /// <summary>
    /// Convertit une entité <see cref="City"/> en <see cref="CityDto"/>.
    /// </summary>
    /// <param name="entity">Entité ville à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static CityDto ToDto(this City entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CityDto(
            entity.Id,
            entity.Name,
            entity.CountryId,
            entity.Latitude,
            entity.Longitude           
        );
    }

    /// <summary>
    /// Convertit un <see cref="CityDto"/> en entité <see cref="City"/>.
    /// </summary>
    /// <param name="dto">DTO ville à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static City ToEntity(this CityDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new City(
            dto.Id,
            dto.Name,
            dto.Latitude,
            dto.Longitude,
            dto.CountryId
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="City"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this City entity, CityDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;
        entity.CountryId = dto.CountryId;
    }

    #endregion

    #region Country

    /// <summary>
    /// Convertit une entité <see cref="Country"/> en <see cref="CountryDto"/>.
    /// </summary>
    /// <param name="entity">Entité pays à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static CountryDto ToDto(this Country entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CountryDto(
            entity.Id,
            entity.Name,
            entity.Iso2,
            entity.Iso3
        );
    }

    /// <summary>
    /// Convertit un <see cref="CountryDto"/> en entité <see cref="Country"/>.
    /// </summary>
    /// <param name="dto">DTO pays à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Country ToEntity(this CountryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Country(
            dto.Id,
            dto.Name,
            dto.Iso2,
            dto.Iso3
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="Country"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Country entity, CountryDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.Iso2 = dto.Iso2;
        entity.Iso3 = dto.Iso3;
    }

    #endregion

    #region Flight

    /// <summary>
    /// Convertit une entité <see cref="Flight"/> en <see cref="FlightDto"/>.
    /// </summary>
    /// <param name="entity">Entité vol à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static FlightDto ToDto(this Domain.Entities.Flight entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new FlightDto(
            entity.Id,
            entity.Code,
            entity.Departure,
            entity.EstimatedArrival,
            entity.BusinessClassSlots,
            entity.EconomySlots,
            entity.BusinessClassPrice,
            entity.EconomyPrice,
            entity.To,
            entity.From
        );
    }

    /// <summary>
    /// Convertit un <see cref="FlightDto"/> en entité <see cref="Flight"/>.
    /// </summary>
    /// <param name="dto">DTO vol à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Domain.Entities.Flight ToEntity(this FlightDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Domain.Entities.Flight(
            dto.Id,
            dto.Code,
            dto.Departure,
            dto.EstimatedArrival,
            dto.BusinessClassSlots,
            dto.EconomySlots,
            dto.BusinessClassPrice,
            dto.EconomyPrice,
            dto.To,
            dto.From
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="Flight"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Domain.Entities.Flight entity, FlightDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Code = dto.Code;
        entity.Departure = dto.Departure;
        entity.EstimatedArrival = dto.EstimatedArrival;
        entity.BusinessClassSlots = dto.BusinessClassSlots;
        entity.EconomySlots = dto.EconomySlots;
        entity.BusinessClassPrice = dto.BusinessClassPrice;
        entity.EconomyPrice = dto.EconomyPrice;
        entity.To = dto.To;
        entity.From = dto.From;
    }

    #endregion

    #region Passenger

    /// <summary>
    /// Convertit une entité <see cref="Passenger"/> en <see cref="PassengerDto"/>.
    /// </summary>
    /// <param name="entity">Entité passager à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static PassengerDto ToDto(this Passenger entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new PassengerDto(
            entity.Id,
            entity.Name,
            entity.MiddleName,
            entity.LastName,
            entity.Email,
            entity.Contact,
            entity.Address,
            entity.Sex
        );
    }

    /// <summary>
    /// Convertit un <see cref="PassengerDto"/> en entité <see cref="Passenger"/>.
    /// </summary>
    /// <param name="dto">DTO passager à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Passenger ToEntity(this PassengerDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Passenger(
            dto.Id,
            dto.Name,
            dto.MiddleName,
            dto.LastName,
            dto.Email,
            dto.Contact,
            dto.Address,
            dto.Sex
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="Passenger"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Passenger entity, PassengerDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.MiddleName = dto.MiddleName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
        entity.Contact = dto.Contact;
        entity.Address = dto.Address;
        entity.Sex = dto.Sex;
    }

    #endregion

    #region Vehicle

    /// <summary>
    /// Convertit une entité <see cref="Vehicle"/> en <see cref="VehicleDto"/>.
    /// </summary>
    /// <param name="entity">Entité véhicule à convertir.</param>
    /// <returns>DTO correspondant à l'entité fournie.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static VehicleDto ToDto(this Vehicle entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new VehicleDto(
            entity.Id,
            entity.LicensePlate,
            entity.Manufacturer,
            entity.Model,
            entity.Year,
            entity.Tariff
        );
    }

    /// <summary>
    /// Convertit un <see cref="VehicleDto"/> en entité <see cref="Vehicle"/>.
    /// </summary>
    /// <param name="dto">DTO véhicule à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Vehicle ToEntity(this VehicleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Vehicle(
            dto.Id,
            dto.LicensePlate,
            dto.Manufacturer,
            dto.Model,
            dto.Year,
            dto.Tariff
        );
    }

    /// <summary>
    /// Met à jour une entité <see cref="Vehicle"/> à partir d'un DTO.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <param name="dto">DTO source contenant les nouvelles valeurs.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Vehicle entity, VehicleDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.LicensePlate = dto.LicensePlate;
        entity.Manufacturer = dto.Manufacturer;
        entity.Model = dto.Model;
        entity.Year = dto.Year;
        entity.Tariff = dto.Tariff;
    }

    #endregion
}