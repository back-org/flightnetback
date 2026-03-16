/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Airports' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Airports;

public record UpdateAirportCommand(int Id, AirportDto Dto, string? PerformedBy = null) : IRequest<AirportDto?>;

public class UpdateAirportCommandHandler : IRequestHandler<UpdateAirportCommand, AirportDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateAirportCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<AirportDto?> Handle(UpdateAirportCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Airport.GetByIdAsync(request.Id);
        if (existing is null) return null;

        existing.UpdateEntity(request.Dto);
        await _manager.Airport.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Airport",
            entityId: existing.Id.ToString(),
            details: $"Aéroport mis à jour: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}