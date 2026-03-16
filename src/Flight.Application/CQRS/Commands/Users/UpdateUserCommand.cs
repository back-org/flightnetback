using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Users;

/// <summary>
/// Commande permettant de mettre à jour un utilisateur existant.
/// </summary>
/// <param name="Id">Identifiant de l'utilisateur à modifier.</param>
/// <param name="Dto">Nouvelles données.</param>
/// <param name="PerformedBy">Nom de l'utilisateur ou système ayant lancé l'action.</param>
public record UpdateUserCommand(int Id, UserDto Dto, string PerformedBy) : IRequest<UserDto?>;

/// <summary>
/// Handler chargé de mettre à jour un utilisateur.
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateUserCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    /// <inheritdoc />
    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.User.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.User.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "User",
            entityId: existing.Id.ToString(),
            details: $"Utilisateur mis à jour : {existing.UserName}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}