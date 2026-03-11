using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="Airline"/>.
/// </summary>
public static class AirlineExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="Airline"/> en <see cref="AirlineDto"/>.
    /// </summary>
    /// <param name="airline">L'entité compagnie aérienne à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static AirlineDto ToDto(this Airline airline)
    {
        return new AirlineDto(airline.Id, airline.Name, airline.State, airline.DeletedFlag);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour une compagnie aérienne.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant de la compagnie (0 pour une création).</param>
/// <param name="Name">Nom de la compagnie aérienne.</param>
/// <param name="State">État d'activité de la compagnie.</param>
/// <param name="DeletedFlag">Drapeau de suppression logique.</param>
public record AirlineDto(int Id, string Name, State State, int DeletedFlag);

/// <summary>
/// Représente une compagnie aérienne dans le système.
/// </summary>
[Table("Airlines")]
public class Airline : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Airline"/>.
    /// </summary>
    public Airline()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Airline"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données de la compagnie.</param>
    public Airline(AirlineDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Nom officiel de la compagnie aérienne.
    /// </summary>
    [Required(ErrorMessage = "Le nom de la compagnie aérienne est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom ne peut pas dépasser 30 caractères.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// État d'activité de la compagnie aérienne (Active ou Inactive).
    /// </summary>
    [Required(ErrorMessage = "L'état de la compagnie est requis.")]
    [Column("state")]
    [JsonProperty(PropertyName = "state")]
    public State State { get; set; } = State.Active;

    /// <summary>
    /// Indicateur de suppression logique. Une valeur non nulle signale une suppression douce.
    /// </summary>
    [Column("flag")]
    [JsonProperty(PropertyName = "flag")]
    public int DeletedFlag { get; set; }

    /// <summary>
    /// Copie les valeurs d'un <see cref="AirlineDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(AirlineDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        Name = dto.Name;
        State = dto.State;
        DeletedFlag = dto.DeletedFlag;
    }
}
