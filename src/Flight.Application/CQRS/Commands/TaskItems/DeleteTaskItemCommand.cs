using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.TaskItems;

/// <summary>
/// Commande permettant de supprimer une tâche.
/// </summary>
public record DeleteTaskItemCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer une tâche.
/// </summary>
public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteTaskItemCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.TaskItem.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.TaskItem.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "TaskItem",
            entityId: request.Id.ToString(),
            details: $"Tâche supprimée : {existing.Title}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}