using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Flight.Application.Applications;
using Flight.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant l'authentification JWT des utilisateurs.
/// Gère la connexion, déconnexion, le renouvellement de token et l'impersonation.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AccountController(
    ILogger<AccountController> logger,
    IUserService userService,
    IJwtAuthManager jwtAuthManager)
    : ControllerBase
{
    /// <summary>
    /// Authentifie un utilisateur et retourne un token JWT d'accès ainsi qu'un refresh token.
    /// </summary>
    /// <param name="request">Les identifiants de connexion (nom d'utilisateur et mot de passe).</param>
    /// <returns>
    /// Un <see cref="LoginResult"/> contenant les tokens JWT si l'authentification réussit,
    /// ou un 401 Unauthorized si les identifiants sont invalides.
    /// </returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!userService.IsValidUserCredentials(request.UserName, request.Password))
            return Unauthorized(new { message = "Nom d'utilisateur ou mot de passe incorrect." });

        var role = userService.GetUserRole(request.UserName);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtResult = jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
        logger.LogInformation("User [{UserName}] logged into the system.", request.UserName);

        return Ok(new LoginResult
        {
            UserName = request.UserName,
            Role = role,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }

    /// <summary>
    /// Retourne les informations de l'utilisateur actuellement authentifié.
    /// </summary>
    /// <returns>Un <see cref="LoginResult"/> contenant le nom, le rôle et le nom d'origine (en cas d'impersonation).</returns>
    [HttpGet("user")]
    [Authorize]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult GetCurrentUser()
    {
        return Ok(new LoginResult
        {
            UserName = User.Identity?.Name ?? string.Empty,
            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
            OriginalUserName = User.FindFirst("OriginalUserName")?.Value ?? string.Empty
        });
    }

    /// <summary>
    /// Déconnecte l'utilisateur en invalidant son refresh token côté serveur.
    /// </summary>
    /// <returns>200 OK si la déconnexion a réussi.</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult Logout()
    {
        var userName = User.Identity?.Name ?? string.Empty;
        jwtAuthManager.RemoveRefreshTokenByUserName(userName);
        logger.LogInformation("User [{UserName}] logged out of the system.", userName);
        return Ok(new { message = "Déconnexion réussie." });
    }

    /// <summary>
    /// Renouvelle le token d'accès JWT à l'aide d'un refresh token valide.
    /// </summary>
    /// <param name="request">Le refresh token à utiliser pour le renouvellement.</param>
    /// <returns>
    /// Un nouveau <see cref="LoginResult"/> avec un token d'accès et un refresh token renouvelés,
    /// ou 401 si le refresh token est invalide ou expiré.
    /// </returns>
    [HttpPost("refresh-token")]
    [Authorize]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var userName = User.Identity?.Name ?? string.Empty;
            logger.LogInformation("User [{UserName}] is trying to refresh JWT token.", userName);

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Unauthorized(new { message = "Le refresh token est requis." });

            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            var jwtResult = jwtAuthManager.Refresh(request.RefreshToken, accessToken ?? string.Empty, DateTime.Now);

            logger.LogInformation("User [{UserName}] has refreshed JWT token.", userName);

            return Ok(new LoginResult
            {
                UserName = userName,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(new { message = e.Message });
        }
    }

    /// <summary>
    /// Permet à un administrateur d'emprunter l'identité d'un autre utilisateur (impersonation).
    /// Un admin ne peut pas usurper l'identité d'un autre admin.
    /// </summary>
    /// <param name="request">Le nom de l'utilisateur à usurper.</param>
    /// <returns>
    /// Un <see cref="LoginResult"/> avec les tokens de l'utilisateur impersonné,
    /// ou 400 si l'utilisateur cible est introuvable ou est un admin.
    /// </returns>
    [HttpPost("impersonation")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Impersonate([FromBody] ImpersonationRequest request)
    {
        var userName = User.Identity?.Name ?? string.Empty;
        logger.LogInformation("User [{UserName}] is trying to impersonate [{TargetUser}].", userName, request.UserName);

        var impersonatedRole = userService.GetUserRole(request.UserName);
        if (string.IsNullOrWhiteSpace(impersonatedRole))
        {
            logger.LogWarning("Impersonation failed: target user [{TargetUser}] not found.", request.UserName);
            return BadRequest(new { message = $"L'utilisateur [{request.UserName}] est introuvable." });
        }

        if (impersonatedRole == UserRoles.Admin)
        {
            logger.LogWarning("User [{UserName}] attempted to impersonate another admin.", userName);
            return BadRequest(new { message = "Il n'est pas possible d'usurper l'identité d'un autre administrateur." });
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, impersonatedRole),
            new Claim("OriginalUserName", userName)
        };

        var jwtResult = jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
        logger.LogInformation("User [{UserName}] is now impersonating [{TargetUser}].", userName, request.UserName);

        return Ok(new LoginResult
        {
            UserName = request.UserName,
            Role = impersonatedRole,
            OriginalUserName = userName,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }

    /// <summary>
    /// Arrête l'impersonation en cours et restaure la session de l'administrateur original.
    /// </summary>
    /// <returns>
    /// Un <see cref="LoginResult"/> avec les tokens de l'administrateur d'origine,
    /// ou 400 si aucune impersonation n'est en cours.
    /// </returns>
    [HttpPost("stop-impersonation")]
    [Authorize]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult StopImpersonation()
    {
        var userName = User.Identity?.Name ?? string.Empty;
        var originalUserName = User.FindFirst("OriginalUserName")?.Value;

        if (string.IsNullOrWhiteSpace(originalUserName))
            return BadRequest(new { message = "Aucune impersonation n'est actuellement en cours." });

        logger.LogInformation("User [{OriginalUser}] is stopping impersonation of [{ImpersonatedUser}].", originalUserName, userName);

        var role = userService.GetUserRole(originalUserName);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, originalUserName),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtResult = jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
        logger.LogInformation("User [{OriginalUser}] has stopped impersonation.", originalUserName);

        return Ok(new LoginResult
        {
            UserName = originalUserName,
            Role = role,
            OriginalUserName = string.Empty,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }
}

/// <summary>
/// Modèle de requête pour la connexion d'un utilisateur.
/// </summary>
public class LoginRequest
{
    /// <summary>Nom d'utilisateur.</summary>
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>Mot de passe de l'utilisateur.</summary>
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Modèle de réponse après une authentification ou un renouvellement de token réussi.
/// </summary>
public class LoginResult
{
    /// <summary>Nom d'utilisateur authentifié.</summary>
    [JsonPropertyName("username")] public string UserName { get; set; } = string.Empty;

    /// <summary>Rôle de l'utilisateur (Admin ou BasicUser).</summary>
    [JsonPropertyName("role")] public string Role { get; set; } = string.Empty;

    /// <summary>Nom de l'utilisateur original en cas d'impersonation active.</summary>
    [JsonPropertyName("originalUserName")] public string OriginalUserName { get; set; } = string.Empty;

    /// <summary>Token JWT d'accès (Bearer).</summary>
    [JsonPropertyName("accessToken")] public string AccessToken { get; set; } = string.Empty;

    /// <summary>Token de renouvellement (refresh token).</summary>
    [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Modèle de requête pour le renouvellement du token JWT.
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>Le refresh token à utiliser pour obtenir un nouveau access token.</summary>
    [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Modèle de requête pour l'impersonation d'un utilisateur.
/// </summary>
public class ImpersonationRequest
{
    /// <summary>Nom de l'utilisateur cible à usurper.</summary>
    [JsonPropertyName("username")] public string UserName { get; set; } = string.Empty;
}
