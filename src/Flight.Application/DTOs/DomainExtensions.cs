/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

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

#region Baggage

/// <summary>
/// Convertit une entité <see cref="Baggage"/> en <see cref="BaggageDto"/>.
/// </summary>
public static BaggageDto ToDto(this Baggage entity)
{
    ArgumentNullException.ThrowIfNull(entity);

    return new BaggageDto(
        entity.Id,
        entity.BookingId,
        entity.PassengerId,
        entity.FlightId,
        entity.TagNumber,
        entity.Weight,
        entity.Type,
        entity.Status
    );
}

/// <summary>
/// Convertit un <see cref="BaggageDto"/> en entité <see cref="Baggage"/>.
/// </summary>
public static Baggage ToEntity(this BaggageDto dto)
{
    ArgumentNullException.ThrowIfNull(dto);

    return new Baggage
    {
        Id = dto.Id,
        BookingId = dto.BookingId,
        PassengerId = dto.PassengerId,
        FlightId = dto.FlightId,
        TagNumber = dto.TagNumber,
        Weight = dto.Weight,
        Type = dto.Type,
        Status = dto.Status
    };
}

/// <summary>
/// Met à jour une entité <see cref="Baggage"/> à partir d'un <see cref="BaggageDto"/>.
/// </summary>
public static void UpdateEntity(this Baggage entity, BaggageDto dto)
{
    ArgumentNullException.ThrowIfNull(entity);
    ArgumentNullException.ThrowIfNull(dto);

    entity.BookingId = dto.BookingId;
    entity.PassengerId = dto.PassengerId;
    entity.FlightId = dto.FlightId;
    entity.TagNumber = dto.TagNumber;
    entity.Weight = dto.Weight;
    entity.Type = dto.Type;
    entity.Status = dto.Status;
}

#endregion

#region RefreshToken

/// <summary>
/// Convertit une entité <see cref="RefreshToken"/> en <see cref="RefreshTokenDto"/>.
/// </summary>
public static RefreshTokenDto ToDto(this RefreshToken entity)
{
    ArgumentNullException.ThrowIfNull(entity);

    return new RefreshTokenDto(
        entity.Id,
        entity.UserId,
        entity.Token,
        entity.ExpiresAt,
        entity.IsRevoked,
        entity.CreatedAt
    );
}

/// <summary>
/// Convertit un <see cref="RefreshTokenDto"/> en entité <see cref="RefreshToken"/>.
/// </summary>
public static RefreshToken ToEntity(this RefreshTokenDto dto)
{
    ArgumentNullException.ThrowIfNull(dto);

    return new RefreshToken
    {
        Id = dto.Id,
        UserId = dto.UserId,
        Token = dto.Token,
        ExpiresAt = dto.ExpiresAt,
        IsRevoked = dto.IsRevoked,
        CreatedAt = dto.CreatedAt
    };
}

/// <summary>
/// Met à jour une entité <see cref="RefreshToken"/> à partir d'un <see cref="RefreshTokenDto"/>.
/// </summary>
public static void UpdateEntity(this RefreshToken entity, RefreshTokenDto dto)
{
    ArgumentNullException.ThrowIfNull(entity);
    ArgumentNullException.ThrowIfNull(dto);

    entity.UserId = dto.UserId;
    entity.Token = dto.Token;
    entity.ExpiresAt = dto.ExpiresAt;
    entity.IsRevoked = dto.IsRevoked;
    entity.CreatedAt = dto.CreatedAt;
}

#endregion

#region TaskItem

/// <summary>
/// Convertit une entité <see cref="TaskItem"/> en <see cref="TaskItemDto"/>.
/// </summary>
public static TaskItemDto ToDto(this TaskItem entity)
{
    ArgumentNullException.ThrowIfNull(entity);

    return new TaskItemDto(
        entity.Id,
        entity.Title,
        entity.Description,
        entity.CreatedByUserId,
        entity.AssignedToUserId,
        entity.Priority,
        entity.Status,
        entity.DueDate,
        entity.CreatedAt
    );
}

/// <summary>
/// Convertit un <see cref="TaskItemDto"/> en entité <see cref="TaskItem"/>.
/// </summary>
public static TaskItem ToEntity(this TaskItemDto dto)
{
    ArgumentNullException.ThrowIfNull(dto);

    return new TaskItem
    {
        Id = dto.Id,
        Title = dto.Title,
        Description = dto.Description,
        CreatedByUserId = dto.CreatedByUserId,
        AssignedToUserId = dto.AssignedToUserId,
        Priority = dto.Priority,
        Status = dto.Status,
        DueDate = dto.DueDate,
        CreatedAt = dto.CreatedAt
    };
}

/// <summary>
/// Met à jour une entité <see cref="TaskItem"/> à partir d'un <see cref="TaskItemDto"/>.
/// </summary>
public static void UpdateEntity(this TaskItem entity, TaskItemDto dto)
{
    ArgumentNullException.ThrowIfNull(entity);
    ArgumentNullException.ThrowIfNull(dto);

    entity.Title = dto.Title;
    entity.Description = dto.Description;
    entity.CreatedByUserId = dto.CreatedByUserId;
    entity.AssignedToUserId = dto.AssignedToUserId;
    entity.Priority = dto.Priority;
    entity.Status = dto.Status;
    entity.DueDate = dto.DueDate;
    entity.CreatedAt = dto.CreatedAt;
}

#endregion

#region SeatAssignment

/// <summary>
/// Convertit une entité <see cref="SeatAssignment"/> en <see cref="SeatAssignmentDto"/>.
/// </summary>
public static SeatAssignmentDto ToDto(this SeatAssignment entity)
{
    ArgumentNullException.ThrowIfNull(entity);

    return new SeatAssignmentDto(
        entity.Id,
        entity.FlightId,
        entity.PassengerId,
        entity.SeatNumber,
        entity.SeatClass
    );
}

/// <summary>
/// Convertit un <see cref="SeatAssignmentDto"/> en entité <see cref="SeatAssignment"/>.
/// </summary>
public static SeatAssignment ToEntity(this SeatAssignmentDto dto)
{
    ArgumentNullException.ThrowIfNull(dto);

    return new SeatAssignment
    {
        Id = dto.Id,
        FlightId = dto.FlightId,
        PassengerId = dto.PassengerId,
        SeatNumber = dto.SeatNumber,
        SeatClass = dto.SeatClass
    };
}

/// <summary>
/// Met à jour une entité <see cref="SeatAssignment"/> à partir d'un <see cref="SeatAssignmentDto"/>.
/// </summary>
public static void UpdateEntity(this SeatAssignment entity, SeatAssignmentDto dto)
{
    ArgumentNullException.ThrowIfNull(entity);
    ArgumentNullException.ThrowIfNull(dto);

    entity.FlightId = dto.FlightId;
    entity.PassengerId = dto.PassengerId;
    entity.SeatNumber = dto.SeatNumber;
    entity.SeatClass = dto.SeatClass;
}

#endregion

#region Ticket

/// <summary>
/// Convertit une entité <see cref="Ticket"/> en <see cref="TicketDto"/>.
/// </summary>
/// <param name="entity">Entité billet à convertir.</param>
/// <returns>DTO billet correspondant.</returns>
/// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
public static TicketDto ToDto(this Ticket entity)
{
    ArgumentNullException.ThrowIfNull(entity);

    return new TicketDto(
        entity.Id,
        entity.TicketNumber,
        entity.BookingId,
        entity.PassengerId,
        entity.IssuedAt,
        entity.Status
    );
}

/// <summary>
/// Convertit un <see cref="TicketDto"/> en entité <see cref="Ticket"/>.
/// </summary>
/// <param name="dto">DTO billet à convertir.</param>
/// <returns>Entité billet correspondante.</returns>
/// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
public static Ticket ToEntity(this TicketDto dto)
{
    ArgumentNullException.ThrowIfNull(dto);

    return new Ticket
    {
        Id = dto.Id,
        TicketNumber = dto.TicketNumber,
        BookingId = dto.BookingId,
        PassengerId = dto.PassengerId,
        IssuedAt = dto.IssuedAt,
        Status = dto.Status
    };
}

/// <summary>
/// Met à jour une entité <see cref="Ticket"/> à partir d'un <see cref="TicketDto"/>.
/// </summary>
/// <param name="entity">Entité billet existante.</param>
/// <param name="dto">DTO source.</param>
/// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
public static void UpdateEntity(this Ticket entity, TicketDto dto)
{
    ArgumentNullException.ThrowIfNull(entity);
    ArgumentNullException.ThrowIfNull(dto);

    entity.TicketNumber = dto.TicketNumber;
    entity.BookingId = dto.BookingId;
    entity.PassengerId = dto.PassengerId;
    entity.IssuedAt = dto.IssuedAt;
    entity.Status = dto.Status;
}

#endregion


    #region User

    /// <summary>
    /// Convertit une entité <see cref="User"/> en <see cref="UserDto"/>.
    /// </summary>
    /// <param name="entity">Entité utilisateur à convertir.</param>
    /// <returns>DTO utilisateur correspondant.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static UserDto ToDto(this User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new UserDto(
            entity.Id,
            entity.UserName,
            entity.Email,
            entity.FirstName,
            entity.LastName,
            entity.PhoneNumber,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.LastLoginAt
        );
    }

    /// <summary>
    /// Convertit un <see cref="UserDto"/> en entité <see cref="User"/>.
    /// </summary>
    /// <param name="dto">DTO utilisateur à convertir.</param>
    /// <returns>Entité utilisateur correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static User ToEntity(this UserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new User
        {
            Id = dto.Id,
            UserName = dto.UserName,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            LastLoginAt = dto.LastLoginAt,

            // Important :
            // PasswordHash n'est pas exposé dans UserDto.
            // Il devra être géré séparément dans le processus de création/modification utilisateur.
            PasswordHash = string.Empty
        };
    }

    /// <summary>
    /// Met à jour une entité <see cref="User"/> à partir d'un <see cref="UserDto"/>.
    /// </summary>
    /// <param name="entity">Entité utilisateur existante.</param>
    /// <param name="dto">DTO source.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this User entity, UserDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.UserName = dto.UserName;
        entity.Email = dto.Email;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.IsActive = dto.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.LastLoginAt = dto.LastLoginAt;
    }

    #endregion

    #region Role

    /// <summary>
    /// Convertit une entité <see cref="Role"/> en <see cref="RoleDto"/>.
    /// </summary>
    /// <param name="entity">Entité rôle à convertir.</param>
    /// <returns>DTO rôle correspondant.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static RoleDto ToDto(this Role entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new RoleDto(
            entity.Id,
            entity.Name,
            entity.Description
        );
    }

    /// <summary>
    /// Convertit un <see cref="RoleDto"/> en entité <see cref="Role"/>.
    /// </summary>
    /// <param name="dto">DTO rôle à convertir.</param>
    /// <returns>Entité rôle correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Role ToEntity(this RoleDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Role
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description
        };
    }

    /// <summary>
    /// Met à jour une entité <see cref="Role"/> à partir d'un <see cref="RoleDto"/>.
    /// </summary>
    /// <param name="entity">Entité rôle existante.</param>
    /// <param name="dto">DTO source.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Role entity, RoleDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Name = dto.Name;
        entity.Description = dto.Description;
    }

    #endregion

    #region CrewMember

    /// <summary>
    /// Convertit une entité <see cref="CrewMember"/> en <see cref="CrewMemberDto"/>.
    /// </summary>
    /// <param name="entity">Entité membre d'équipe à convertir.</param>
    /// <returns>DTO correspondant.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static CrewMemberDto ToDto(this CrewMember entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CrewMemberDto(
            entity.Id,
            entity.UserId,
            entity.EmployeeNumber,
            entity.Position,
            entity.LicenseNumber,
            entity.HireDate,
            entity.Status
        );
    }

    /// <summary>
    /// Convertit un <see cref="CrewMemberDto"/> en entité <see cref="CrewMember"/>.
    /// </summary>
    /// <param name="dto">DTO membre d'équipe à convertir.</param>
    /// <returns>Entité correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static CrewMember ToEntity(this CrewMemberDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new CrewMember
        {
            Id = dto.Id,
            UserId = dto.UserId,
            EmployeeNumber = dto.EmployeeNumber,
            Position = dto.Position,
            LicenseNumber = dto.LicenseNumber,
            HireDate = dto.HireDate,
            Status = dto.Status
        };
    }

    /// <summary>
    /// Met à jour une entité <see cref="CrewMember"/> à partir d'un <see cref="CrewMemberDto"/>.
    /// </summary>
    /// <param name="entity">Entité membre d'équipe existante.</param>
    /// <param name="dto">DTO source.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this CrewMember entity, CrewMemberDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.UserId = dto.UserId;
        entity.EmployeeNumber = dto.EmployeeNumber;
        entity.Position = dto.Position;
        entity.LicenseNumber = dto.LicenseNumber;
        entity.HireDate = dto.HireDate;
        entity.Status = dto.Status;
    }

    #endregion

    #region Aircraft

    /// <summary>
    /// Convertit une entité <see cref="Aircraft"/> en <see cref="AircraftDto"/>.
    /// </summary>
    /// <param name="entity">Entité avion à convertir.</param>
    /// <returns>DTO avion correspondant.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static AircraftDto ToDto(this Aircraft entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AircraftDto(
            entity.Id,
            entity.RegistrationNumber,
            entity.Manufacturer,
            entity.Model,
            entity.BusinessCapacity,
            entity.EconomyCapacity,
            entity.Status,
            entity.LastMaintenanceDate
        );
    }

    /// <summary>
    /// Convertit un <see cref="AircraftDto"/> en entité <see cref="Aircraft"/>.
    /// </summary>
    /// <param name="dto">DTO avion à convertir.</param>
    /// <returns>Entité avion correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Aircraft ToEntity(this AircraftDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Aircraft
        {
            Id = dto.Id,
            RegistrationNumber = dto.RegistrationNumber,
            Manufacturer = dto.Manufacturer,
            Model = dto.Model,
            BusinessCapacity = dto.BusinessCapacity,
            EconomyCapacity = dto.EconomyCapacity,
            Status = dto.Status,
            LastMaintenanceDate = dto.LastMaintenanceDate
        };
    }

    /// <summary>
    /// Met à jour une entité <see cref="Aircraft"/> à partir d'un <see cref="AircraftDto"/>.
    /// </summary>
    /// <param name="entity">Entité avion existante.</param>
    /// <param name="dto">DTO source.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Aircraft entity, AircraftDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.RegistrationNumber = dto.RegistrationNumber;
        entity.Manufacturer = dto.Manufacturer;
        entity.Model = dto.Model;
        entity.BusinessCapacity = dto.BusinessCapacity;
        entity.EconomyCapacity = dto.EconomyCapacity;
        entity.Status = dto.Status;
        entity.LastMaintenanceDate = dto.LastMaintenanceDate;
    }

    #endregion

    #region Payment

    /// <summary>
    /// Convertit une entité <see cref="Payment"/> en <see cref="PaymentDto"/>.
    /// </summary>
    /// <param name="entity">Entité paiement à convertir.</param>
    /// <returns>DTO paiement correspondant.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static PaymentDto ToDto(this Payment entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new PaymentDto(
            entity.Id,
            entity.BookingId,
            entity.Amount,
            entity.Currency,
            entity.PaymentMethod,
            entity.Status,
            entity.TransactionReference,
            entity.PaidAt
        );
    }

    /// <summary>
    /// Convertit un <see cref="PaymentDto"/> en entité <see cref="Payment"/>.
    /// </summary>
    /// <param name="dto">DTO paiement à convertir.</param>
    /// <returns>Entité paiement correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Payment ToEntity(this PaymentDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Payment
        {
            Id = dto.Id,
            BookingId = dto.BookingId,
            Amount = dto.Amount,
            Currency = dto.Currency,
            PaymentMethod = dto.PaymentMethod,
            Status = dto.Status,
            TransactionReference = dto.TransactionReference,
            PaidAt = dto.PaidAt
        };
    }

    /// <summary>
    /// Met à jour une entité <see cref="Payment"/> à partir d'un <see cref="PaymentDto"/>.
    /// </summary>
    /// <param name="entity">Entité paiement existante.</param>
    /// <param name="dto">DTO source.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Payment entity, PaymentDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.BookingId = dto.BookingId;
        entity.Amount = dto.Amount;
        entity.Currency = dto.Currency;
        entity.PaymentMethod = dto.PaymentMethod;
        entity.Status = dto.Status;
        entity.TransactionReference = dto.TransactionReference;
        entity.PaidAt = dto.PaidAt;
    }

    #endregion

    #region Notification

    /// <summary>
    /// Convertit une entité <see cref="Notification"/> en <see cref="NotificationDto"/>.
    /// </summary>
    /// <param name="entity">Entité notification à convertir.</param>
    /// <returns>DTO notification correspondant.</returns>
    /// <exception cref="ArgumentNullException">Levée si l'entité est nulle.</exception>
    public static NotificationDto ToDto(this Notification entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new NotificationDto(
            entity.Id,
            entity.UserId,
            entity.Subject,
            entity.Message,
            entity.Channel,
            entity.Status,
            entity.CreatedAt,
            entity.SentAt
        );
    }

    /// <summary>
    /// Convertit un <see cref="NotificationDto"/> en entité <see cref="Notification"/>.
    /// </summary>
    /// <param name="dto">DTO notification à convertir.</param>
    /// <returns>Entité notification correspondante.</returns>
    /// <exception cref="ArgumentNullException">Levée si le DTO est nul.</exception>
    public static Notification ToEntity(this NotificationDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Notification
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Subject = dto.Subject,
            Message = dto.Message,
            Channel = dto.Channel,
            Status = dto.Status,
            CreatedAt = dto.CreatedAt,
            SentAt = dto.SentAt
        };
    }

    /// <summary>
    /// Met à jour une entité <see cref="Notification"/> à partir d'un <see cref="NotificationDto"/>.
    /// </summary>
    /// <param name="entity">Entité notification existante.</param>
    /// <param name="dto">DTO source.</param>
    /// <exception cref="ArgumentNullException">Levée si l'entité ou le DTO est nul.</exception>
    public static void UpdateEntity(this Notification entity, NotificationDto dto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(dto);

        entity.Id = dto.Id;
        entity.UserId = dto.UserId;
        entity.Subject = dto.Subject;
        entity.Message = dto.Message;
        entity.Channel = dto.Channel;
        entity.Status = dto.Status;
        entity.CreatedAt = dto.CreatedAt;
        entity.SentAt = dto.SentAt;
    }

    #endregion

}
