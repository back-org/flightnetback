/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Aircrafts' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Aircrafts;

/// <summary>
/// Commande permettant de supprimer un avion.
/// </summary>
public record DeleteAircraftCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer un avion.
/// </summary>
public class DeleteAircraftCommandHandler : IRequestHandler<DeleteAircraftCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteAircraftCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteAircraftCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Aircraft.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.Aircraft.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Aircraft",
            entityId: request.Id.ToString(),
            details: $"Avion supprimé : {existing.RegistrationNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}