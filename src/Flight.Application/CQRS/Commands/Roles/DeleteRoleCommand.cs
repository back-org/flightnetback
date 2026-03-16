using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Roles;

/// <summary>
/// Commande permettant de supprimer un rôle.
/// </summary>
public record DeleteRoleCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer un rôle.
/// </summary>
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteRoleCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    /// <inheritdoc />
    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Role.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.Role.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Role",
            entityId: request.Id.ToString(),
            details: $"Rôle supprimé : {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}