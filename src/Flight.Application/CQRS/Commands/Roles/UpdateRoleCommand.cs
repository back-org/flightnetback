/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Roles' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Roles;

/// <summary>
/// Commande permettant de mettre à jour un rôle existant.
/// </summary>
public record UpdateRoleCommand(int Id, RoleDto Dto, string PerformedBy) : IRequest<RoleDto?>;

/// <summary>
/// Handler chargé de mettre à jour un rôle.
/// </summary>
public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateRoleCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    /// <inheritdoc />
    public async Task<RoleDto?> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Role.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Role.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Role",
            entityId: existing.Id.ToString(),
            details: $"Rôle mis à jour : {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}