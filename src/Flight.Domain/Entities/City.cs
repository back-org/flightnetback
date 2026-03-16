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
/// Représente une ville dans le système.
/// </summary>
[Table("Cities")]
public partial class City : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="City"/>.
    /// </summary>
    public City()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="City"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique de la ville.</param>
    /// <param name="name">Nom officiel de la ville.</param>
    /// <param name="lat">Latitude géographique.</param>
    /// <param name="lon">Longitude géographique.</param>
    /// <param name="countryId">Identifiant du pays de rattachement.</param>
    public City(int id, string name, double lat, double lon, int countryId)
    {
        Id = id;
        Name = name;
        Latitude = lat;
        Longitude = lon;
        CountryId = countryId;
    }

    /// <summary>
    /// Obtient ou définit le nom officiel de la ville.
    /// </summary>
    [Required(ErrorMessage = "Le nom de la ville est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom de la ville ne peut pas dépasser 30 caractères.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la latitude géographique de la ville.
    /// </summary>
    [Column("lat")]
    [JsonProperty(PropertyName = "lat")]
    public double Latitude { get; set; } = 0;

    /// <summary>
    /// Obtient ou définit la longitude géographique de la ville.
    /// </summary>
    [Column("lon")]
    [JsonProperty(PropertyName = "lon")]
    public double Longitude { get; set; } = 0;

    /// <summary>
    /// Obtient ou définit l'identifiant du pays auquel appartient la ville.
    /// </summary>
    [Column("country_id")]
    [JsonProperty(PropertyName = "countryId")]
    public int CountryId { get; set; }
}