using System.ComponentModel.DataAnnotations;
using Flight.Domain.Entities;

namespace Flight.Application.DTOs;

/// <summary>
/// Représente les données transférées pour un aéroport.
/// Ce DTO est utilisé pour la lecture, la création et la mise à jour
/// des aéroports via l'API.
/// </summary>
public class AirportDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="AirportDto"/>.
    /// Ce constructeur est nécessaire pour la sérialisation et la désérialisation JSON.
    /// </summary>
    public AirportDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="AirportDto"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique de l'aéroport.</param>
    /// <param name="name">Nom officiel de l'aéroport.</param>
    /// <param name="cityId">Identifiant de la ville associée à l'aéroport.</param>
    /// <param name="state">État opérationnel de l'aéroport.</param>
    /// <param name="deletedFlag">Indicateur de suppression logique.</param>
    public AirportDto(int id, string name, int cityId, State state, int deletedFlag)
    {
        Id = id;
        Name = name;
        CityId = cityId;
        State = state;
        DeletedFlag = deletedFlag;
    }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'aéroport.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtient ou définit le nom officiel de l'aéroport.
    /// </summary>
    [Required(ErrorMessage = "Le nom de l'aéroport est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom de l'aéroport ne peut pas dépasser 30 caractères.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'identifiant de la ville associée à l'aéroport.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "L'identifiant de la ville doit être supérieur à 0.")]
    public int CityId { get; set; }

    /// <summary>
    /// Obtient ou définit l'état opérationnel de l'aéroport.
    /// </summary>
    public State State { get; set; } = State.Active;

    /// <summary>
    /// Obtient ou définit l'indicateur de suppression logique.
    /// </summary>
    public int DeletedFlag { get; set; }
}