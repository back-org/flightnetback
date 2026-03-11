using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="Flight"/>.
/// </summary>
public static class FlightExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="Flight"/> en <see cref="FlightDto"/>.
    /// </summary>
    /// <param name="flight">L'entité vol à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static FlightDto ToDto(this Flight flight)
    {
        return new FlightDto(
            flight.Id,
            flight.Code,
            flight.Departure,
            flight.EstimatedArrival,
            flight.BusinessClassSlots,
            flight.EconomySlots,
            flight.BusinessClassPrice,
            flight.EconomyPrice,
            flight.To,
            flight.From);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour un vol.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant du vol (0 pour une création).</param>
/// <param name="Code">Code IATA ou interne du vol.</param>
/// <param name="Departure">Date et heure de départ.</param>
/// <param name="EstimatedArrival">Date et heure d'arrivée estimée.</param>
/// <param name="BusinessClassSlots">Nombre de sièges disponibles en classe affaires.</param>
/// <param name="EconomySlots">Nombre de sièges disponibles en classe économique.</param>
/// <param name="BusinessClassPrice">Prix du billet en classe affaires.</param>
/// <param name="EconomyPrice">Prix du billet en classe économique.</param>
/// <param name="To">Identifiant de l'aéroport de destination.</param>
/// <param name="From">Identifiant de l'aéroport de départ.</param>
public record FlightDto(
    int Id,
    string Code,
    DateTime Departure,
    DateTime EstimatedArrival,
    int BusinessClassSlots,
    int EconomySlots,
    float BusinessClassPrice,
    float EconomyPrice,
    int To,
    int From);

/// <summary>
/// Représente un vol commercial dans le système.
/// Un vol relie deux aéroports (<see cref="Airport"/>) et peut contenir plusieurs <see cref="Booking"/>.
/// </summary>
[Table("Flights")]
public class Flight : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Flight"/>.
    /// </summary>
    public Flight()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Flight"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données du vol.</param>
    public Flight(FlightDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Code unique du vol (ex : AF1234, MK501).
    /// </summary>
    [Required(ErrorMessage = "Le code du vol est requis.")]
    [MaxLength(30, ErrorMessage = "Le code ne peut pas dépasser 30 caractères.")]
    [Column("code")]
    [JsonProperty(PropertyName = "code")]
    [DataType(DataType.Text)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Date et heure de départ du vol.
    /// </summary>
    [Required(ErrorMessage = "La date de départ est requise.")]
    [Column("departure")]
    [JsonProperty(PropertyName = "departure")]
    [DataType(DataType.DateTime)]
    public DateTime Departure { get; set; }

    /// <summary>
    /// Date et heure d'arrivée estimée du vol.
    /// </summary>
    [Required(ErrorMessage = "La date d'arrivée est requise.")]
    [Column("arrival")]
    [JsonProperty(PropertyName = "arrival")]
    [DataType(DataType.DateTime)]
    public DateTime EstimatedArrival { get; set; }

    /// <summary>
    /// Nombre de sièges disponibles en classe affaires.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Le nombre de sièges affaires doit être positif.")]
    [Column("bus_slot")]
    [JsonProperty(PropertyName = "bus_slot")]
    public int BusinessClassSlots { get; set; }

    /// <summary>
    /// Nombre de sièges disponibles en classe économique.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Le nombre de sièges économiques doit être positif.")]
    [Column("eco_slot")]
    [JsonProperty(PropertyName = "eco_slot")]
    public int EconomySlots { get; set; }

    /// <summary>
    /// Prix unitaire d'un billet en classe affaires.
    /// </summary>
    [Range(0, float.MaxValue, ErrorMessage = "Le prix affaires doit être positif.")]
    [Column("bus_price")]
    [JsonProperty(PropertyName = "bus_price")]
    public float BusinessClassPrice { get; set; }

    /// <summary>
    /// Prix unitaire d'un billet en classe économique.
    /// </summary>
    [Range(0, float.MaxValue, ErrorMessage = "Le prix économique doit être positif.")]
    [Column("eco_price")]
    [JsonProperty(PropertyName = "eco_price")]
    public float EconomyPrice { get; set; }

    /// <summary>
    /// Collection de réservations effectuées sur ce vol.
    /// </summary>
    public virtual ICollection<Booking> Bookings { get; set; } = [];

    /// <summary>
    /// Identifiant de l'aéroport de destination (clé étrangère vers <see cref="Airport"/>).
    /// </summary>
    [Required(ErrorMessage = "L'aéroport de destination est requis.")]
    [Column("flight_to")]
    [JsonProperty(PropertyName = "flight_to")]
    public int To { get; set; }

    /// <summary>
    /// Navigation vers l'entité <see cref="Airport"/> de destination.
    /// </summary>
    [ForeignKey(nameof(To))]
    public virtual Airport? DestinationAirport { get; set; }

    /// <summary>
    /// Identifiant de l'aéroport de départ (clé étrangère vers <see cref="Airport"/>).
    /// </summary>
    [Required(ErrorMessage = "L'aéroport de départ est requis.")]
    [Column("flight_from")]
    [JsonProperty(PropertyName = "flight_from")]
    public int From { get; set; }

    /// <summary>
    /// Navigation vers l'entité <see cref="Airport"/> de départ.
    /// </summary>
    [ForeignKey(nameof(From))]
    public virtual Airport? DepartureAirport { get; set; }

    /// <summary>
    /// Copie les valeurs d'un <see cref="FlightDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(FlightDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        Code = dto.Code;
        Departure = dto.Departure;
        EstimatedArrival = dto.EstimatedArrival;
        BusinessClassSlots = dto.BusinessClassSlots;
        EconomySlots = dto.EconomySlots;
        BusinessClassPrice = dto.BusinessClassPrice;
        EconomyPrice = dto.EconomyPrice;
        To = dto.To;
        From = dto.From;
    }
}
