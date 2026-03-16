/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Airports' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Airports;

public record DeleteAirportCommand(int Id, string? PerformedBy = null) : IRequest<bool>;

public class DeleteAirportCommandHandler : IRequestHandler<DeleteAirportCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteAirportCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteAirportCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Airport.GetByIdAsync(request.Id);
        if (existing is null) return false;

        await _manager.Airport.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Airport",
            entityId: request.Id.ToString(),
            details: $"Aéroport supprimé: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}