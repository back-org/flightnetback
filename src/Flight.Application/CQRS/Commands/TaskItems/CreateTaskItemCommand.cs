/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/TaskItems' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.TaskItems;

/// <summary>
/// Commande permettant de créer une tâche.
/// </summary>
public record CreateTaskItemCommand(TaskItemDto Dto, string PerformedBy) : IRequest<TaskItemDto>;

/// <summary>
/// Handler chargé de créer une tâche.
/// </summary>
public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, TaskItemDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateTaskItemCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<TaskItemDto> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.TaskItem.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "TaskItem",
            entityId: entity.Id.ToString(),
            details: $"Tâche créée : {entity.Title}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}