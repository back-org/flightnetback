using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="Booking"/>.
/// </summary>
public static class BookingExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="Booking"/> en <see cref="BookingDto"/>.
    /// </summary>
    /// <param name="booking">L'entité réservation à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static BookingDto ToDto(this Booking booking)
    {
        return new BookingDto(
            booking.Id,
            booking.FlightType,
            booking.FlightId,
            booking.PassengerId,
            booking.Statut);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour une réservation.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant de la réservation (0 pour une création).</param>
/// <param name="FlightType">Classe de confort choisie.</param>
/// <param name="FlightId">Identifiant du vol concerné.</param>
/// <param name="PassengerId">Identifiant du passager concerné.</param>
/// <param name="Statut">Statut actuel de la réservation.</param>
public record BookingDto(int Id, Confort FlightType, int FlightId, int PassengerId, Statut Statut);

/// <summary>
/// Représente une réservation de vol dans le système.
/// Associe un passager à un vol avec une classe de confort et un statut.
/// </summary>
[Table("Bookings")]
public class Booking : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Booking"/>.
    /// </summary>
    public Booking()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Booking"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données de la réservation.</param>
    public Booking(BookingDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Classe de confort choisie par le passager (Économique, Affaires, etc.).
    /// </summary>
    [Column("flight_type")]
    [JsonProperty(PropertyName = "flight_type")]
    public Confort FlightType { get; set; } = Confort.Economy;

    /// <summary>
    /// Identifiant du vol associé à cette réservation (clé étrangère vers <see cref="Flight"/>).
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du vol est requis.")]
    [Column("flight_id")]
    [JsonProperty(PropertyName = "flight_id")]
    public int FlightId { get; set; }

    /// <summary>
    /// Navigation vers l'entité <see cref="Flight"/> associée.
    /// </summary>
    [ForeignKey(nameof(FlightId))]
    public virtual Flight? Plane { get; set; }

    /// <summary>
    /// Identifiant du passager ayant effectué la réservation (clé étrangère vers <see cref="Passenger"/>).
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du passager est requis.")]
    [Column("passenger_id")]
    [JsonProperty(PropertyName = "passenger_id")]
    public int PassengerId { get; set; }

    /// <summary>
    /// Navigation vers l'entité <see cref="Passenger"/> associée.
    /// </summary>
    [ForeignKey(nameof(PassengerId))]
    public virtual Passenger? Passenger { get; set; }

    /// <summary>
    /// Statut actuel de la réservation (En attente, Confirmée, Annulée).
    /// </summary>
    [Column("state")]
    [JsonProperty(PropertyName = "state")]
    public Statut Statut { get; set; } = Statut.Pending;

    /// <summary>
    /// Copie les valeurs d'un <see cref="BookingDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(BookingDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        FlightType = dto.FlightType;
        FlightId = dto.FlightId;
        PassengerId = dto.PassengerId;
        Statut = dto.Statut;
    }
}
