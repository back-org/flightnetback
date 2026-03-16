/*
 * Rôle métier du fichier: Composant applicatif.
 * Description: Ce fichier participe au sous-domaine 'Flight.Infrastructure/AuditTrail' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Domain.Entities;
using Flight.Infrastructure.Database;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flight.Infrastructure.AuditTrail;

/// <summary>
/// Implémentation du service d'audit trail.
/// Persiste chaque opération sensible en base de données.
/// </summary>
public class AuditTrailService : IAuditTrailService
{
    private readonly FlightContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuditTrailService> _logger;

    public AuditTrailService(
        FlightContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuditTrailService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task RecordAsync(
        string action,
        string entityName,
        string entityId,
        string details,
        string? performedBy = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = performedBy
                       ?? httpContext?.User?.Identity?.Name
                       ?? "anonymous";

            var ip = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";

            var log = new AuditLog
            {
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Details = details,
                PerformedBy = user,
                IpAddress = ip,
                PerformedAt = DateTime.UtcNow,
                Success = true
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[AUDIT] {Action} sur {Entity}#{EntityId} par {User} depuis {IP}",
                action, entityName, entityId, user, ip);
        }
        catch (Exception ex)
        {
            // L'audit ne doit jamais interrompre le flux principal
            _logger.LogError(ex, "[AUDIT] Erreur lors de l'enregistrement de l'audit");
        }
    }

  
}
