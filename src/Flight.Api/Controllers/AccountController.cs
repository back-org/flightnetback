/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.Applications;
using Flight.Infrastructure.Auth;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de l'authentification, de la gestion des tokens JWT,
/// de la récupération de l'utilisateur courant et de l'impersonation.
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AccountController(
    ILogger<AccountController> logger,
    IUserService userService,
    IJwtAuthManager jwtAuthManager,
    IAuditTrailService audit)
    : ControllerBase
{
    /// <summary>
    /// Authentifie un utilisateur et retourne un access token ainsi qu'un refresh token.
    /// </summary>
    /// <param name="request">Informations d'identification de l'utilisateur.</param>
    /// <returns>Informations de session de l'utilisateur connecté.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    [EndpointName("Login")]
    [EndpointSummary("Authentifier un utilisateur")]
    [EndpointDescription("Vérifie les identifiants de l'utilisateur et retourne un access token JWT ainsi qu'un refresh token.")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Le modèle de connexion est invalide.",
                Detail = "Le nom d'utilisateur et le mot de passe sont requis.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        if (!userService.IsValidUserCredentials(request.UserName, request.Password))
        {
            await audit.RecordAsync(
                action: "LOGIN_FAILED",
                entityName: "Account",
                entityId: request.UserName,
                details: $"Tentative de connexion échouée pour {request.UserName}");

            return Unauthorized(new ErrorResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Nom d'utilisateur ou mot de passe incorrect.",
                Detail = "Les identifiants fournis sont invalides.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        var role = userService.GetUserRole(request.UserName);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtResult = jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);

        await audit.RecordAsync(
            action: "LOGIN",
            entityName: "Account",
            entityId: request.UserName,
            details: $"Connexion réussie pour {request.UserName} (rôle: {role})");

        logger.LogInformation("Utilisateur [{UserName}] connecté.", request.UserName);

        return Ok(new LoginResult
        {
            UserName = request.UserName,
            Role = role,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }

    /// <summary>
    /// Retourne les informations de l'utilisateur authentifié courant.
    /// </summary>
    /// <returns>Informations de l'utilisateur connecté.</returns>
    [HttpGet("user")]
    [EndpointName("GetCurrentUser")]
    [EndpointSummary("Obtenir l'utilisateur courant")]
    [EndpointDescription("Retourne les informations de l'utilisateur authentifié courant à partir des claims JWT.")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    public ActionResult<LoginResult> GetCurrentUser()
    {
        return Ok(new LoginResult
        {
            UserName = User.Identity?.Name ?? string.Empty,
            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
            OriginalUserName = User.FindFirst("OriginalUserName")?.Value ?? string.Empty
        });
    }

    /// <summary>
    /// Déconnecte l'utilisateur courant en invalidant son refresh token.
    /// </summary>
    /// <returns>Message confirmant la déconnexion.</returns>
    [HttpPost("logout")]
    [EndpointName("Logout")]
    [EndpointSummary("Déconnecter l'utilisateur")]
    [EndpointDescription("Invalide le refresh token de l'utilisateur connecté et enregistre une trace d'audit.")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<MessageResponse>> Logout()
    {
        var userName = User.Identity?.Name ?? string.Empty;
        jwtAuthManager.RemoveRefreshTokenByUserName(userName);

        await audit.RecordAsync(
            action: "LOGOUT",
            entityName: "Account",
            entityId: userName,
            details: $"Déconnexion de {userName}");

        logger.LogInformation("Utilisateur [{UserName}] déconnecté.", userName);

        return Ok(new MessageResponse
        {
            Message = "Déconnexion réussie."
        });
    }

    /// <summary>
    /// Renouvelle le token d'accès JWT à partir d'un refresh token valide.
    /// </summary>
    /// <param name="request">Refresh token transmis par le client.</param>
    /// <returns>Nouveaux tokens d'accès et de rafraîchissement.</returns>
    [HttpPost("refresh-token")]
    [EndpointName("RefreshToken")]
    [EndpointSummary("Renouveler le token JWT")]
    [EndpointDescription("Renouvelle le token d'accès JWT à l'aide d'un refresh token encore valide.")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResult>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var userName = User.Identity?.Name ?? string.Empty;

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return Unauthorized(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Le refresh token est requis.",
                    Detail = "La requête doit contenir un refresh token valide.",
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            var jwtResult = jwtAuthManager.Refresh(request.RefreshToken, accessToken ?? string.Empty, DateTime.Now);

            logger.LogInformation("Utilisateur [{UserName}] a renouvelé son token.", userName);

            return Ok(new LoginResult
            {
                UserName = userName,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new ErrorResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Le refresh token est invalide ou expiré.",
                Detail = ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Permet à un administrateur d'usurper temporairement l'identité d'un autre utilisateur.
    /// </summary>
    /// <param name="request">Nom d'utilisateur cible de l'impersonation.</param>
    /// <returns>Nouveaux tokens associés à l'utilisateur impersoné.</returns>
    [HttpPost("impersonation")]
    [Authorize(Roles = "Admin")]
    [EndpointName("ImpersonateUser")]
    [EndpointSummary("Démarrer une impersonation")]
    [EndpointDescription("Permet à un administrateur d'endosser temporairement l'identité d'un autre utilisateur non administrateur.")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResult>> Impersonate([FromBody] ImpersonationRequest request)
    {
        var userName = User.Identity?.Name ?? string.Empty;
        var impersonatedRole = userService.GetUserRole(request.UserName);

        if (string.IsNullOrWhiteSpace(impersonatedRole))
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Utilisateur introuvable.",
                Detail = $"L'utilisateur [{request.UserName}] est introuvable.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        if (impersonatedRole == UserRoles.Admin)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Impersonation interdite.",
                Detail = "Il n'est pas possible d'usurper l'identité d'un autre administrateur.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, impersonatedRole),
            new Claim("OriginalUserName", userName)
        };

        var jwtResult = jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);

        await audit.RecordAsync(
            action: "IMPERSONATION_START",
            entityName: "Account",
            entityId: request.UserName,
            details: $"Admin {userName} commence l'impersonation de {request.UserName}");

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
    /// Arrête l'impersonation en cours et restaure l'identité originale.
    /// </summary>
    /// <returns>Nouveaux tokens associés à l'utilisateur d'origine.</returns>
    [HttpPost("stop-impersonation")]
    [EndpointName("StopImpersonation")]
    [EndpointSummary("Arrêter l'impersonation")]
    [EndpointDescription("Met fin à l'impersonation en cours et restaure l'identité initiale de l'utilisateur d'origine.")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResult>> StopImpersonation()
    {
        var userName = User.Identity?.Name ?? string.Empty;
        var originalUserName = User.FindFirst("OriginalUserName")?.Value;

        if (string.IsNullOrWhiteSpace(originalUserName))
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Aucune impersonation en cours.",
                Detail = "L'utilisateur courant n'est pas en train d'usurper une autre identité.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        var role = userService.GetUserRole(originalUserName);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, originalUserName),
            new Claim(ClaimTypes.Role, role)
        };

        var jwtResult = jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);

        await audit.RecordAsync(
            action: "IMPERSONATION_STOP",
            entityName: "Account",
            entityId: originalUserName,
            details: $"{originalUserName} arrête l'impersonation de {userName}");

        return Ok(new LoginResult
        {
            UserName = originalUserName,
            Role = role,
            AccessToken = jwtResult.AccessToken,
            RefreshToken = jwtResult.RefreshToken.TokenString
        });
    }
}

/// <summary>
/// Représente la requête de connexion contenant les identifiants utilisateur.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Nom d'utilisateur utilisé pour l'authentification.
    /// </summary>
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Mot de passe utilisé pour l'authentification.
    /// </summary>
    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Représente le résultat d'une authentification réussie.
/// </summary>
public class LoginResult
{
    /// <summary>
    /// Nom d'utilisateur authentifié.
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Rôle de sécurité associé à l'utilisateur.
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Nom de l'utilisateur d'origine en cas d'impersonation.
    /// </summary>
    [JsonPropertyName("originalUserName")]
    public string OriginalUserName { get; set; } = string.Empty;

    /// <summary>
    /// JWT d'accès permettant d'appeler les endpoints sécurisés.
    /// </summary>
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh token permettant de renouveler le JWT d'accès.
    /// </summary>
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Représente une requête de renouvellement de token.
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh token fourni par le client.
    /// </summary>
    [Required(ErrorMessage = "Le refresh token est requis.")]
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Représente une demande d'impersonation d'utilisateur.
/// </summary>
public class ImpersonationRequest
{
    /// <summary>
    /// Nom d'utilisateur cible de l'impersonation.
    /// </summary>
    [Required(ErrorMessage = "Le nom d'utilisateur cible est requis.")]
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;
}

/// <summary>
/// Représente une réponse simple contenant un message lisible par le client.
/// </summary>
public class MessageResponse
{
    /// <summary>
    /// Message de confirmation ou d'information.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}