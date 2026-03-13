using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;
using System;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un vol dans le système.
/// </summary>
[Table("Flights")]
public partial class Flight : DeleteEntity<int>
{
    

    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Flight"/>.
    /// </summary>
    public Flight()
    {
    }  

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Flight"/> avec les valeurs fournies.
    /// </summary>
    public Flight(
        int id,
        string code,
        DateTime departure,
        DateTime estimatedArrival,
        int businessClassSlots,
        int economySlots,
        float businessClassPrice,
        float economyPrice,
        int to,
        int from)
    {
        Id = id;
        Code = code;
        Departure = departure;
        EstimatedArrival = estimatedArrival;
        BusinessClassSlots = businessClassSlots;
        EconomySlots = economySlots;
        BusinessClassPrice = businessClassPrice;
        EconomyPrice = economyPrice;
        To = to;
        From = from;
    }

    /// <summary>
    /// Obtient ou définit le code unique du vol.
    /// </summary>
    [Required(ErrorMessage = "Le code du vol est requis.")]
    [MaxLength(30)]
    [Column("code")]
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la date et l'heure de départ.
    /// </summary>
    [Column("departure")]
    [JsonProperty(PropertyName = "departure")]
    public DateTime Departure { get; set; }

    /// <summary>
    /// Obtient ou définit la date et l'heure estimée d'arrivée.
    /// </summary>
    [Column("estimated_arrival")]
    [JsonProperty(PropertyName = "estimatedArrival")]
    public DateTime EstimatedArrival { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre de places disponibles en classe affaires.
    /// </summary>
    [Column("business_class_slots")]
    [JsonProperty(PropertyName = "businessClassSlots")]
    public int BusinessClassSlots { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre de places disponibles en classe économique.
    /// </summary>
    [Column("economy_slots")]
    [JsonProperty(PropertyName = "economySlots")]
    public int EconomySlots { get; set; }

    /// <summary>
    /// Obtient ou définit le prix d'un billet en classe affaires.
    /// </summary>
    [Column("business_class_price")]
    [JsonProperty(PropertyName = "businessClassPrice")]
    public float BusinessClassPrice { get; set; }

    /// <summary>
    /// Obtient ou définit le prix d'un billet en classe économique.
    /// </summary>
    [Column("economy_price")]
    [JsonProperty(PropertyName = "economyPrice")]
    public float EconomyPrice { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'aéroport de destination.
    /// </summary>
    [Column("to_airport")]
    [JsonProperty(PropertyName = "to")]
    public int To { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'aéroport de départ.
    /// </summary>
    [Column("from_airport")]
    [JsonProperty(PropertyName = "from")]
    public int From { get; set; }
}