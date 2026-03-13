using Microsoft.AspNetCore.Authorization;

namespace Flight.Api.Authorization;

/// <summary>
/// Exigence d'autorisation : l'utilisateur doit être un administrateur actif (non impersonné).
/// </summary>
public class ActiveAdminRequirement : IAuthorizationRequirement { }

/// <summary>
/// Handler pour l'exigence d'administrateur actif.
/// Vérifie que l'utilisateur a le rôle Admin ET n'est pas en session d'impersonation.
/// </summary>
public class ActiveAdminHandler : AuthorizationHandler<ActiveAdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveAdminRequirement requirement)
    {
        var isAdmin = context.User.IsInRole("Admin");
        var isImpersonating = context.User.FindFirst("OriginalUserName") != null;

        if (isAdmin && !isImpersonating)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

/// <summary>
/// Exigence d'autorisation : l'utilisateur doit être propriétaire de la ressource
/// ou avoir le rôle Admin.
/// </summary>
public class ResourceOwnerOrAdminRequirement : IAuthorizationRequirement { }

/// <summary>
/// Handler pour l'exigence propriétaire ou admin.
/// </summary>
public class ResourceOwnerOrAdminHandler : AuthorizationHandler<ResourceOwnerOrAdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceOwnerOrAdminRequirement requirement)
    {
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Pour une implémentation complète, vérifier l'ownership via le resource
        if (context.Resource is string resourceUserId)
        {
            var currentUserId = context.User.Identity?.Name;
            if (currentUserId == resourceUserId)
                context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

/// <summary>
/// Noms des politiques d'autorisation disponibles dans l'application.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>Politique réservée aux administrateurs actifs (non impersonnés).</summary>
    public const string ActiveAdmin = "ActiveAdmin";

    /// <summary>Politique pour les administrateurs ou le propriétaire de la ressource.</summary>
    public const string OwnerOrAdmin = "OwnerOrAdmin";

    /// <summary>Politique pour les opérations de lecture (tous les utilisateurs authentifiés).</summary>
    public const string AuthenticatedRead = "AuthenticatedRead";

    /// <summary>Politique pour les opérations d'écriture (Admin ou BasicUser).</summary>
    public const string WriteAccess = "WriteAccess";
}
