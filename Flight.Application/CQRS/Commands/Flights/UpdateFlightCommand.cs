
using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Flights;

/// <summary>
/// Commande MediatR pour la mise à jour d'un vol.
/// </summary>
public record UpdateFlightCommand(int Id, FlightDto Dto) : IRequest<FlightDto?>;

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
        if (existing is null) return null;

        existing.Copy(request.Dto);
        await _manager.Flight.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Flight",
            entityId: existing.Id.ToString(),
            details: $"Vol mis à jour: {existing.Code}",
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}
