using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Notifications;

/// <summary>
/// Requête permettant de récupérer toutes les notifications.
/// </summary>
public record GetAllNotificationsQuery() : IRequest<IEnumerable<NotificationDto>>;

/// <summary>
/// Handler chargé de récupérer toutes les notifications.
/// </summary>
public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllNotificationsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Notification.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}