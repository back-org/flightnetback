using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Notifications;

/// <summary>
/// Commande permettant de mettre à jour une notification existante.
/// </summary>
public record UpdateNotificationCommand(int Id, NotificationDto Dto, string PerformedBy) : IRequest<NotificationDto?>;

/// <summary>
/// Handler chargé de mettre à jour une notification.
/// </summary>
public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, NotificationDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateNotificationCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<NotificationDto?> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Notification.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Notification.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Notification",
            entityId: existing.Id.ToString(),
            details: $"Notification mise à jour pour utilisateur #{existing.UserId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}