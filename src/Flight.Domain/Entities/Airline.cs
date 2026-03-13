using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente une compagnie aérienne dans le système.
/// </summary>
[Table("Airlines")]
public partial class Airline : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Airline"/>.
    /// </summary>
    public Airline()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Airline"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique de la compagnie aérienne.</param>
    /// <param name="name">Nom officiel de la compagnie aérienne.</param>
    /// <param name="state">État d'activité de la compagnie aérienne.</param>
    /// <param name="deletedFlag">Indicateur de suppression logique.</param>
    public Airline(int id, string name, State state, int deletedFlag)
    {
        Id = id;
        Name = name;
        State = state;
        DeletedFlag = deletedFlag;
    }

    /// <summary>
    /// Obtient ou définit le nom officiel de la compagnie aérienne.
    /// </summary>
    [Required(ErrorMessage = "Le nom de la compagnie aérienne est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom ne peut pas dépasser 30 caractères.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'état d'activité de la compagnie aérienne.
    /// </summary>
    [Required(ErrorMessage = "L'état de la compagnie est requis.")]
    [Column("state")]
    [JsonProperty(PropertyName = "state")]
    public State State { get; set; } = State.Active;

    /// <summary>
    /// Obtient ou définit l'indicateur de suppression logique.
    /// Une valeur non nulle indique une suppression douce.
    /// </summary>
    [Column("flag")]
    [JsonProperty(PropertyName = "flag")]
    public int DeletedFlag { get; set; }
}