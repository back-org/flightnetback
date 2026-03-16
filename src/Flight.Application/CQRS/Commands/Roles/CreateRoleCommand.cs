using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Roles;

/// <summary>
/// Commande permettant de créer un nouveau rôle.
/// </summary>
public record CreateRoleCommand(RoleDto Dto, string PerformedBy) : IRequest<RoleDto>;

/// <summary>
/// Handler chargé de créer un nouveau rôle.
/// </summary>
public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateRoleCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    /// <inheritdoc />
    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Role.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Role",
            entityId: entity.Id.ToString(),
            details: $"Rôle créé : {entity.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}