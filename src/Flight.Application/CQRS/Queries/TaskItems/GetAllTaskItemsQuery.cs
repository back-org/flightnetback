/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/TaskItems' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.TaskItems;

/// <summary>
/// Requête permettant de récupérer toutes les tâches.
/// </summary>
public record GetAllTaskItemsQuery() : IRequest<IEnumerable<TaskItemDto>>;

/// <summary>
/// Handler chargé de récupérer toutes les tâches.
/// </summary>
public class GetAllTaskItemsQueryHandler : IRequestHandler<GetAllTaskItemsQuery, IEnumerable<TaskItemDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllTaskItemsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<TaskItemDto>> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.TaskItem.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}