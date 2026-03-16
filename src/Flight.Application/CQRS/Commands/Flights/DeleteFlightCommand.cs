/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Flights' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Flights;

/// <summary>
/// Commande MediatR pour la suppression d'un vol.
/// </summary>
/// <param name="Id">Identifiant du vol à supprimer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record DeleteFlightCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler pour la commande de suppression de vol.
/// </summary>
public class DeleteFlightCommandHandler : IRequestHandler<DeleteFlightCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteFlightCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Flight.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return false;
        }

        await _manager.Flight.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Flight",
            entityId: request.Id.ToString(),
            details: $"Vol supprimé: {existing.Code}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}