using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Users;

/// <summary>
/// Commande permettant de supprimer un utilisateur.
/// </summary>
/// <param name="Id">Identifiant de l'utilisateur à supprimer.</param>
/// <param name="PerformedBy">Nom de l'utilisateur ou système ayant lancé l'action.</param>
public record DeleteUserCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer un utilisateur.
/// </summary>
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteUserCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    /// <inheritdoc />
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.User.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.User.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "User",
            entityId: request.Id.ToString(),
            details: $"Utilisateur supprimé : {existing.UserName}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}