/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/TaskItems' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.TaskItems;

/// <summary>
/// Commande permettant de mettre à jour une tâche existante.
/// </summary>
public record UpdateTaskItemCommand(int Id, TaskItemDto Dto, string PerformedBy) : IRequest<TaskItemDto?>;

/// <summary>
/// Handler chargé de mettre à jour une tâche.
/// </summary>
public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, TaskItemDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateTaskItemCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<TaskItemDto?> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.TaskItem.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.TaskItem.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "TaskItem",
            entityId: existing.Id.ToString(),
            details: $"Tâche mise à jour : {existing.Title}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}