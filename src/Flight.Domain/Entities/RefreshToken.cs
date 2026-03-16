using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un refresh token lié à un utilisateur.
/// Il permet de renouveler un token d'accès sans se reconnecter à chaque fois.
/// </summary>
[Table("RefreshTokens")]
public class RefreshToken
{
    /// <summary>
    /// Identifiant unique du refresh token.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur propriétaire du token.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant utilisateur est requis.")]
    public int UserId { get; set; }

    /// <summary>
    /// Valeur du refresh token.
    /// </summary>
    [Required(ErrorMessage = "La valeur du token est requise.")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Date d'expiration du refresh token.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Indique si le refresh token a été révoqué.
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// Date de création du refresh token.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}