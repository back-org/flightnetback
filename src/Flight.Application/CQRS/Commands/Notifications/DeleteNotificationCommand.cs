/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Notifications' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Notifications;

/// <summary>
/// Commande permettant de supprimer une notification.
/// </summary>
public record DeleteNotificationCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer une notification.
/// </summary>
public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteNotificationCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Notification.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.Notification.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Notification",
            entityId: request.Id.ToString(),
            details: $"Notification supprimée pour utilisateur #{existing.UserId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}