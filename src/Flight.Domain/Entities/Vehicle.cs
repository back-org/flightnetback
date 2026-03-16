/*
 * Rôle métier du fichier: Modéliser les entités métier du domaine aérien.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Entities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un véhicule dans le système.
/// </summary>
[Table("Vehicles")]
public partial class Vehicle : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Vehicle"/>.
    /// </summary>
    public Vehicle()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Vehicle"/> avec les valeurs fournies.
    /// </summary>
    public Vehicle(int id, string licensePlate, string manufacturer, string model, short year, float tariff)
    {
        Id = id;
        LicensePlate = licensePlate;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Tariff = tariff;
    }

    /// <summary>
    /// Obtient ou définit la plaque d'immatriculation du véhicule.
    /// </summary>
    [Required]
    [MaxLength(20)]
    [Column("license_plate")]
    [JsonProperty(PropertyName = "licensePlate")]
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le fabricant du véhicule.
    /// </summary>
    [Required]
    [Column("manufacturer")]
    [JsonProperty(PropertyName = "manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le modèle du véhicule.
    /// </summary>
    [Required]
    [Column("model")]
    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'année de fabrication du véhicule.
    /// </summary>
    [Column("year")]
    [JsonProperty(PropertyName = "year")]
    public short Year { get; set; }

    /// <summary>
    /// Obtient ou définit le tarif appliqué au véhicule.
    /// </summary>
    [Column("tariff")]
    [JsonProperty(PropertyName = "tariff")]
    public float Tariff { get; set; }
}