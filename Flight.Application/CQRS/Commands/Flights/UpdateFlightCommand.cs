using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Flights;

/// <summary>
/// Commande MediatR pour la mise à jour d'un vol.
/// </summary>
/// <param name="Id">Identifiant du vol à mettre à jour.</param>
/// <param name="Dto">Données mises à jour du vol.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record UpdateFlightCommand(int Id, FlightDto Dto, string PerformedBy) : IRequest<FlightDto?>;

/// <summary>
/// Handler pour la commande de mise à jour de vol.
/// </summary>
public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, FlightDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateFlightCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<FlightDto?> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Flight.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Flight.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Flight",
            entityId: existing.Id.ToString(),
            details: $"Vol mis à jour: {existing.Code}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}