/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Notifications' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Notifications;

/// <summary>
/// Commande permettant de créer une notification.
/// </summary>
public record CreateNotificationCommand(NotificationDto Dto, string PerformedBy) : IRequest<NotificationDto>;

/// <summary>
/// Handler chargé de créer une notification.
/// </summary>
public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateNotificationCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Notification.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Notification",
            entityId: entity.Id.ToString(),
            details: $"Notification créée pour utilisateur #{entity.UserId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}