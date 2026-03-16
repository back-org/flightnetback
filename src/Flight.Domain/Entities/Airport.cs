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
/// Représente un aéroport dans le système.
/// </summary>
[Table("Airports")]
public partial class Airport : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Airport"/>.
    /// </summary>
    public Airport()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Airport"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique de l'aéroport.</param>
    /// <param name="name">Nom officiel de l'aéroport.</param>
    /// <param name="cityId">Identifiant de la ville associée à l'aéroport.</param>
    /// <param name="state">État opérationnel de l'aéroport.</param>
    /// <param name="deletedFlag">Indicateur de suppression logique.</param>
    public Airport(int id, string name, int cityId, State state, int deletedFlag)
    {
        Id = id;
        Name = name;
        CityId = cityId;
        State = state;
        DeletedFlag = deletedFlag;
    }

    /// <summary>
    /// Obtient ou définit le nom officiel de l'aéroport.
    /// </summary>
    [Required(ErrorMessage = "Le nom de l'aéroport est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom de l'aéroport ne peut pas dépasser 30 caractères.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'identifiant de la ville associée à l'aéroport.
    /// </summary>
    [Column("city_id")]
    [JsonProperty(PropertyName = "cityId")]
    public int CityId { get; set; }

    /// <summary>
    /// Obtient ou définit l'état opérationnel de l'aéroport.
    /// </summary>
    [Column("state")]
    [JsonProperty(PropertyName = "state")]
    public State State { get; set; } = State.Active;

    /// <summary>
    /// Obtient ou définit l'indicateur de suppression logique.
    /// </summary>
    [Column("flag")]
    [JsonProperty(PropertyName = "flag")]
    public int DeletedFlag { get; set; }
}