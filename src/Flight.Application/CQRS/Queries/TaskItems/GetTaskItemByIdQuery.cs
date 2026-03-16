/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/TaskItems' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.TaskItems;

/// <summary>
/// Requête permettant de récupérer une tâche par son identifiant.
/// </summary>
public record GetTaskItemByIdQuery(int Id) : IRequest<TaskItemDto?>;

/// <summary>
/// Handler chargé de récupérer une tâche par identifiant.
/// </summary>
public class GetTaskItemByIdQueryHandler : IRequestHandler<GetTaskItemByIdQuery, TaskItemDto?>
{
    private readonly IRepositoryManager _manager;

    public GetTaskItemByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<TaskItemDto?> Handle(GetTaskItemByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.TaskItem.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}