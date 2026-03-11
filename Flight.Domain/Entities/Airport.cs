using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="Airport"/>.
/// </summary>
public static class AirportExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="Airport"/> en <see cref="AirportDto"/>.
    /// </summary>
    /// <param name="airport">L'entité aéroport à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static AirportDto ToDto(this Airport airport)
    {
        return new AirportDto(airport.Id, airport.Name, airport.State, airport.DeletedFlag);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour un aéroport.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant de l'aéroport (0 pour une création).</param>
/// <param name="Name">Nom de l'aéroport.</param>
/// <param name="State">État opérationnel de l'aéroport.</param>
/// <param name="DeletedFlag">Drapeau de suppression logique.</param>
public record AirportDto(int Id, string Name, State State, int DeletedFlag);

/// <summary>
/// Représente un aéroport dans le système.
/// Un aéroport peut être un point de départ ou d'arrivée pour un <see cref="Flight"/>.
/// </summary>
[Table("Airports")]
public class Airport : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Airport"/>.
    /// </summary>
    public Airport()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Airport"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données de l'aéroport.</param>
    public Airport(AirportDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Nom officiel de l'aéroport.
    /// </summary>
    [Required(ErrorMessage = "Le nom de l'aéroport est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom ne peut pas dépasser 30 caractères.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// État opérationnel de l'aéroport (Actif ou Inactif).
    /// </summary>
    [Column("state")]
    [JsonProperty(PropertyName = "state")]
    public State State { get; set; } = State.Active;

    /// <summary>
    /// Indicateur de suppression logique.
    /// </summary>
    [Column("flag")]
    [JsonProperty(PropertyName = "flag")]
    public int DeletedFlag { get; set; }

    /// <summary>
    /// Copie les valeurs d'un <see cref="AirportDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(AirportDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        Name = dto.Name;
        State = dto.State;
        DeletedFlag = dto.DeletedFlag;
    }
}
