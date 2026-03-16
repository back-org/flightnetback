/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Airlines' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Airlines;

/// <summary>
/// Commande MediatR pour mettre à jour une compagnie aérienne.
/// </summary>
public record UpdateAirlineCommand(int Id, AirlineDto Dto, string? PerformedBy = null) : IRequest<AirlineDto?>;

/// <summary>
/// Handler pour la mise à jour d'une compagnie aérienne.
/// </summary>
public class UpdateAirlineCommandHandler : IRequestHandler<UpdateAirlineCommand, AirlineDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateAirlineCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<AirlineDto?> Handle(UpdateAirlineCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Airline.GetByIdAsync(request.Id);
        if (existing is null) return null;

        existing.UpdateEntity(request.Dto);
        await _manager.Airline.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Airline",
            entityId: existing.Id.ToString(),
            details: $"Compagnie mise à jour: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}