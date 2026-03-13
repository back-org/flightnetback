using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente une réservation effectuée par un passager pour un vol donné.
/// </summary>
[Table("Bookings")]
public partial class Booking : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Booking"/>.
    /// </summary>
    public Booking()
    {
    }
    

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Booking"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique de la réservation.</param>
    /// <param name="flightType">Classe de confort réservée.</param>
    /// <param name="flightId">Identifiant du vol réservé.</param>
    /// <param name="passengerId">Identifiant du passager concerné.</param>
    /// <param name="statut">Statut actuel de la réservation.</param>
    public Booking(int id, Confort flightType, int flightId, int passengerId, Statut statut)
    {
        Id = id;
        FlightType = flightType;
        FlightId = flightId;
        PassengerId = passengerId;
        Statut = statut;
    }

    /// <summary>
    /// Obtient ou définit la classe de confort réservée.
    /// </summary>
    [Column("flight_type")]
    [JsonProperty(PropertyName = "flightType")]
    public Confort FlightType { get; set; } = Confort.Economy;

    /// <summary>
    /// Obtient ou définit l'identifiant du vol associé à la réservation.
    /// </summary>
    [Required(ErrorMessage = "Le vol est requis.")]
    [Column("flight_id")]
    [JsonProperty(PropertyName = "flightId")]
    public int FlightId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du passager associé à la réservation.
    /// </summary>
    [Required(ErrorMessage = "Le passager est requis.")]
    [Column("passenger_id")]
    [JsonProperty(PropertyName = "passengerId")]
    public int PassengerId { get; set; }

    /// <summary>
    /// Obtient ou définit le statut actuel de la réservation.
    /// </summary>
    [Column("statut")]
    [JsonProperty(PropertyName = "statut")]
    public Statut Statut { get; set; } = Statut.Pending;
   
}