/*
 * Rôle métier du fichier: Sécuriser l’accès applicatif (authentification, autorisation, tokens).
 * Description: Ce fichier participe au sous-domaine 'Flight.Infrastructure/Auth' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace Flight.Infrastructure.Auth;

/// <summary>
/// Interface du gestionnaire d'authentification JWT.
/// Fournit les méthodes de génération, renouvellement et révocation des tokens.
/// </summary>
public interface IJwtAuthManager
{
    /// <summary>Dictionnaire en lecture seule des refresh tokens actifs, indexés par leur valeur.</summary>
    IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }

    /// <summary>Génère un nouveau token d'accès et un refresh token pour l'utilisateur.</summary>
    JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);

    /// <summary>Renouvelle le token d'accès à partir d'un refresh token valide.</summary>
    JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now);

    /// <summary>Supprime tous les refresh tokens expirés.</summary>
    void RemoveExpiredRefreshTokens(DateTime now);

    /// <summary>Révoque tous les refresh tokens d'un utilisateur (lors de la déconnexion).</summary>
    void RemoveRefreshTokenByUserName(string userName);

    /// <summary>Décode et valide un token JWT, retournant les claims et le token parsé.</summary>
    (ClaimsPrincipal, JwtSecurityToken?) DecodeJwtToken(string token);
}

/// <summary>
/// Implémentation du gestionnaire JWT. Gère la génération, la validation et la révocation des tokens.
/// Les refresh tokens sont stockés en mémoire dans un <see cref="ConcurrentDictionary{TKey,TValue}"/>.
/// </summary>
/// <remarks>
/// Pour un déploiement multi-instances, remplacez le stockage en mémoire par Redis ou une base de données.
/// </remarks>
public class JwtAuthManager(JwtTokenConfig jwtTokenConfig) : IJwtAuthManager
{
    /// <inheritdoc/>
    public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary
        => _usersRefreshTokens.ToImmutableDictionary();

    /// <summary>Stockage thread-safe des refresh tokens actifs (clé = token string).</summary>
    private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens = new();

    /// <summary>Clé secrète encodée en ASCII pour la signature HMAC-SHA256.</summary>
    private readonly byte[] _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);

    /// <summary>
    /// Supprime tous les refresh tokens dont la date d'expiration est passée.
    /// Appelé périodiquement par <see cref="JwtRefreshTokenCache"/>.
    /// </summary>
    /// <param name="now">La date/heure de référence pour déterminer l'expiration.</param>
    public void RemoveExpiredRefreshTokens(DateTime now)
    {
        var expiredTokens = _usersRefreshTokens
            .Where(x => x.Value.ExpireAt < now)
            .ToList();

        foreach (var expiredToken in expiredTokens)
            _usersRefreshTokens.TryRemove(expiredToken.Key, out _);
    }

    /// <summary>
    /// Révoque tous les refresh tokens associés à un nom d'utilisateur.
    /// Typiquement appelé lors de la déconnexion de l'utilisateur.
    /// </summary>
    /// <param name="userName">Le nom d'utilisateur dont les tokens doivent être révoqués.</param>
    public void RemoveRefreshTokenByUserName(string userName)
    {
        var refreshTokens = _usersRefreshTokens
            .Where(x => x.Value.UserName == userName)
            .ToList();

        foreach (var refreshToken in refreshTokens)
            _usersRefreshTokens.TryRemove(refreshToken.Key, out _);
    }

    /// <summary>
    /// Génère un token d'accès JWT et un refresh token pour l'utilisateur spécifié.
    /// </summary>
    /// <param name="username">Le nom d'utilisateur (sub du JWT).</param>
    /// <param name="claims">Les claims à inclure dans le JWT (rôle, nom, etc.).</param>
    /// <param name="now">La date/heure de référence pour le calcul des expirations.</param>
    /// <returns>Un <see cref="JwtAuthResult"/> contenant le token d'accès et le refresh token.</returns>
    public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
    {
        var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(
            claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

        var jwtToken = new JwtSecurityToken(
            jwtTokenConfig.Issuer,
            shouldAddAudienceClaim ? jwtTokenConfig.Audience : string.Empty,
            claims,
            expires: now.AddMinutes(jwtTokenConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(_secret),
                SecurityAlgorithms.HmacSha256Signature));

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        var refreshToken = new RefreshToken
        {
            UserName = username,
            TokenString = GenerateRefreshTokenString(),
            ExpireAt = now.AddMinutes(jwtTokenConfig.RefreshTokenExpiration)
        };

        _usersRefreshTokens.AddOrUpdate(
            refreshToken.TokenString,
            refreshToken,
            (_, _) => refreshToken);

        return new JwtAuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Renouvelle le token d'accès à partir d'un refresh token valide.
    /// Valide l'algorithme de signature et la correspondance utilisateur.
    /// </summary>
    /// <param name="refreshToken">Le refresh token à utiliser pour le renouvellement.</param>
    /// <param name="accessToken">Le token d'accès expiré (pour décoder les claims).</param>
    /// <param name="now">La date/heure de référence.</param>
    /// <returns>Un nouveau <see cref="JwtAuthResult"/>.</returns>
    /// <exception cref="SecurityTokenException">Levée si l'un des tokens est invalide ou expiré.</exception>
    public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
    {
        var (principal, jwtToken) = DecodeJwtToken(accessToken);

        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            throw new SecurityTokenException("Token invalide : algorithme de signature incorrect.");

        var userName = principal.Identity?.Name;

        if (!_usersRefreshTokens.TryGetValue(refreshToken, out var existingRefreshToken))
            throw new SecurityTokenException("Refresh token invalide ou inexistant.");

        if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < now)
            throw new SecurityTokenException("Refresh token expiré ou ne correspond pas à l'utilisateur.");

        return GenerateTokens(userName!, principal.Claims.ToArray(), now);
    }

    /// <summary>
    /// Décode et valide un token JWT en retournant le principal et le token parsé.
    /// </summary>
    /// <param name="token">Le token JWT à décoder.</param>
    /// <returns>Un tuple contenant le <see cref="ClaimsPrincipal"/> et le <see cref="JwtSecurityToken"/> validé.</returns>
    /// <exception cref="SecurityTokenException">Levée si le token est vide ou invalide.</exception>
    public (ClaimsPrincipal, JwtSecurityToken?) DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new SecurityTokenException("Le token ne peut pas être vide.");

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secret),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                },
                out var validatedToken);

        return (principal, validatedToken as JwtSecurityToken);
    }

    /// <summary>
    /// Génère une chaîne aléatoire cryptographiquement sûre de 32 octets encodée en Base64.
    /// Utilisée comme valeur du refresh token.
    /// </summary>
    /// <returns>Une chaîne Base64 de 44 caractères.</returns>
    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

/// <summary>
/// Résultat d'une opération d'authentification ou de renouvellement JWT.
/// </summary>
public class JwtAuthResult
{
    /// <summary>Token d'accès JWT (Bearer).</summary>
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>Token de renouvellement associé.</summary>
    [JsonPropertyName("refreshToken")]
    public RefreshToken RefreshToken { get; set; } = new();
}

/// <summary>
/// Représente un refresh token associé à un utilisateur.
/// </summary>
public class RefreshToken
{
    /// <summary>Nom de l'utilisateur propriétaire du token.</summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>Valeur unique et aléatoire du token (Base64, 32 octets).</summary>
    [JsonPropertyName("tokenString")]
    public string TokenString { get; set; } = string.Empty;

    /// <summary>Date et heure d'expiration du token.</summary>
    [JsonPropertyName("expireAt")]
    public DateTime ExpireAt { get; set; }
}
