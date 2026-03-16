/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/CrewMembers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.CrewMembers;

/// <summary>
/// Commande permettant de mettre à jour un membre d'équipe existant.
/// </summary>
public record UpdateCrewMemberCommand(int Id, CrewMemberDto Dto, string PerformedBy) : IRequest<CrewMemberDto?>;

/// <summary>
/// Handler chargé de mettre à jour un membre d'équipe.
/// </summary>
public class UpdateCrewMemberCommandHandler : IRequestHandler<UpdateCrewMemberCommand, CrewMemberDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateCrewMemberCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<CrewMemberDto?> Handle(UpdateCrewMemberCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.CrewMember.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.CrewMember.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "CrewMember",
            entityId: existing.Id.ToString(),
            details: $"Membre d'équipe mis à jour : {existing.EmployeeNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}