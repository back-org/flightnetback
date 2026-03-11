using System.Security.Claims;
using Flight.Application.Applications;
using Flight.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant l'authentification JWT des utilisateurs.
/// </summary>
[ApiController]
[Authorize]
[Asp.Versioning.ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AccountController(
    ILogger<AccountController> logger,
    IUserService userService,
    IJwtAuthManager jwtAuthManager,
    IAuditTrailService audit)
    : ControllerBase
{
    /// <summary>Authentifie un utilisateur et retourne un token JWT.</summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!userService.IsValidUserCredentials(request.UserName, request.Password))
        {
            await audit.RecordAsync(
                action: "LOGIN_FAILED",
                entityName: "Account",
                entityId: request.UserName,
                details: $"Tentative de connexion échouée pour {request.UserName}");

            return Unauthorized(new { message = "Nom d'utilisateur ou mot de passe incorrect." });
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

    /// <summary>Retourne les informations de l'utilisateur courant.</summary>
    [HttpGet("user")]
    [Authorize]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    public ActionResult GetCurrentUser() => Ok(new LoginResult
    {
        UserName = User.Identity?.Name ?? string.Empty,
        Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
        OriginalUserName = User.FindFirst("OriginalUserName")?.Value ?? string.Empty
    });

    /// <summary>Déconnecte l'utilisateur en invalidant son refresh token.</summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Logout()
    {
        var userName = User.Identity?.Name ?? string.Empty;
        jwtAuthManager.RemoveRefreshTokenByUserName(userName);

        await audit.RecordAsync(
            action: "LOGOUT",
            entityName: "Account",
            entityId: userName,
            details: $"Déconnexion de {userName}");

        logger.LogInformation("Utilisateur [{UserName}] déconnecté.", userName);
        return Ok(new { message = "Déconnexion réussie." });
    }

    /// <summary>Renouvelle le token d'accès JWT.</summary>
    [HttpPost("refresh-token")]
    [Authorize]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var userName = User.Identity?.Name ?? string.Empty;
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Unauthorized(new { message = "Le refresh token est requis." });

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
        catch (SecurityTokenException e)
        {
            return Unauthorized(new { message = e.Message });
        }
    }

    /// <summary>Impersonation d'un utilisateur par un administrateur.</summary>
    [HttpPost("impersonation")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Impersonate([FromBody] ImpersonationRequest request)
    {
        var userName = User.Identity?.Name ?? string.Empty;
        var impersonatedRole = userService.GetUserRole(request.UserName);

        if (string.IsNullOrWhiteSpace(impersonatedRole))
            return BadRequest(new { message = $"L'utilisateur [{request.UserName}] est introuvable." });

        if (impersonatedRole == UserRoles.Admin)
            return BadRequest(new { message = "Il n'est pas possible d'usurper l'identité d'un autre administrateur." });

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

    /// <summary>Arrête l'impersonation en cours.</summary>
    [HttpPost("stop-impersonation")]
    [Authorize]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    public async Task<ActionResult> StopImpersonation()
    {
        var userName = User.Identity?.Name ?? string.Empty;
        var originalUserName = User.FindFirst("OriginalUserName")?.Value;

        if (string.IsNullOrWhiteSpace(originalUserName))
            return BadRequest(new { message = "Aucune impersonation n'est actuellement en cours." });

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

public class LoginRequest
{
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResult
{
    [JsonPropertyName("username")] public string UserName { get; set; } = string.Empty;
    [JsonPropertyName("role")] public string Role { get; set; } = string.Empty;
    [JsonPropertyName("originalUserName")] public string OriginalUserName { get; set; } = string.Empty;
    [JsonPropertyName("accessToken")] public string AccessToken { get; set; } = string.Empty;
    [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenRequest
{
    [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; } = string.Empty;
}

public class ImpersonationRequest
{
    [JsonPropertyName("username")] public string UserName { get; set; } = string.Empty;
}
