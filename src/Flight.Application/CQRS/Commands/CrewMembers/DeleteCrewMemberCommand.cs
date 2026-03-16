/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/CrewMembers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.CrewMembers;

/// <summary>
/// Commande permettant de supprimer un membre d'équipe.
/// </summary>
public record DeleteCrewMemberCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer un membre d'équipe.
/// </summary>
public class DeleteCrewMemberCommandHandler : IRequestHandler<DeleteCrewMemberCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteCrewMemberCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteCrewMemberCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.CrewMember.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.CrewMember.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "CrewMember",
            entityId: request.Id.ToString(),
            details: $"Membre d'équipe supprimé : {existing.EmployeeNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}