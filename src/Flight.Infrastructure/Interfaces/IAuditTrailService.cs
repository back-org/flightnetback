namespace Flight.Infrastructure.Interfaces;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Service centralisé d'audit trail pour les opérations sensibles sur toutes les entités.
/// </summary>
public interface IAuditTrailService
{
    /// <summary>
    /// Enregistre une action d'audit.
    /// </summary>
    /// <param name="action">Nom de l'action (CREATE, UPDATE, DELETE, LOGIN, etc.).</param>
    /// <param name="entityName">Nom de l'entité concernée.</param>
    /// <param name="entityId">Identifiant de l'entité concernée.</param>
    /// <param name="details">Détails additionnels sur l'opération.</param>
    /// <param name="performedBy">L'utilisateur ayant effectué l'action.</param>
    /// <param name="cancellationToken">Jeton d'annulation.</param>
    Task RecordAsync(
        string action,
        string entityName,
        string entityId,
        string details,
        string? performedBy = null,
        CancellationToken cancellationToken = default);
}
