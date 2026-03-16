using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente l'association entre un utilisateur et un rôle.
/// Permet d'attribuer un ou plusieurs rôles à un même utilisateur.
/// </summary>
[Table("UserRoles")]
public class UserRole
{
    /// <summary>
    /// Identifiant unique de l'association.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur concerné.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Identifiant du rôle attribué.
    /// </summary>
    [Required]
    public int RoleId { get; set; }
}