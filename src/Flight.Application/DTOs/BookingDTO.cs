using Flight.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant une réservation.
/// </summary>
public class BookingDto
{

    public BookingDto()
    {
    }

    public BookingDto(int id, int passengerId, int flightId, DateTime bookingDate)
    {
        Id = id;
        PassengerId = passengerId;
        FlightId = flightId;
        BookingDate = bookingDate;
    }

    public BookingDto(int id, Confort flightType, int flightId, int passengerId, Statut statut)
    {
        Id = id;
        this.FlightType = flightType;
        FlightId = flightId;
        PassengerId = passengerId;
        this.Statut = statut;
    }

    public int Id { get; set; }

    [Required]
    public int PassengerId { get; set; }

    [Required]
    public int FlightId { get; set; }

    public DateTime BookingDate { get; set; }

    public Confort FlightType { get; set; }
    public Statut Statut { get; set; }


}