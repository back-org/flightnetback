using System.Text.Json.Serialization;

namespace Flight.Infrastructure.Auth;

/// <summary>
/// Classe de configuration pour les tokens JWT de l'application.
/// Les valeurs sont lues depuis la section <c>jwtTokenConfig</c> dans <c>appsettings.json</c>.
/// </summary>
public class JwtTokenConfig
{
    /// <summary>
    /// Clé secrète utilisée pour signer les tokens JWT (algorithme HMAC-SHA256).
    /// Doit faire au moins 32 caractères et être gardée strictement confidentielle.
    /// </summary>
    [JsonPropertyName("secret")]
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant de l'émetteur du token (claim <c>iss</c>).
    /// Exemple : <c>"FlightNetApi"</c>.
    /// </summary>
    [JsonPropertyName("issuer")]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant du destinataire autorisé du token (claim <c>aud</c>).
    /// Exemple : <c>"FlightNetClient"</c>.
    /// </summary>
    [JsonPropertyName("audience")]
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Durée de validité du token d'accès en minutes.
    /// Recommandé : entre 15 et 60 minutes.
    /// </summary>
    [JsonPropertyName("accessTokenExpiration")]
    public int AccessTokenExpiration { get; set; }

    /// <summary>
    /// Durée de validité du refresh token en minutes.
    /// Recommandé : entre 60 minutes et 7 jours selon le contexte.
    /// </summary>
    [JsonPropertyName("refreshTokenExpiration")]
    public int RefreshTokenExpiration { get; set; }
}
