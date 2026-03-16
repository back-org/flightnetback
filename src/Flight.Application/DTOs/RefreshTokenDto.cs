using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un refresh token utilisé pour renouveler une session utilisateur.
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO refresh token.
    /// </summary>
    public RefreshTokenDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO refresh token avec ses valeurs.
    /// </summary>
    public RefreshTokenDto(
        int id,
        int userId,
        string token,
        DateTime expiresAt,
        bool isRevoked,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = isRevoked;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Identifiant unique du refresh token.
    /// </summary>
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
    /// Indique si le token a été révoqué.
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// Date de création du token.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}