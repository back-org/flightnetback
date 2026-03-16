using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Notifications;

/// <summary>
/// Requête permettant de récupérer une notification par son identifiant.
/// </summary>
public record GetNotificationByIdQuery(int Id) : IRequest<NotificationDto?>;

/// <summary>
/// Handler chargé de récupérer une notification par identifiant.
/// </summary>
public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
{
    private readonly IRepositoryManager _manager;

    public GetNotificationByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<NotificationDto?> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Notification.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}