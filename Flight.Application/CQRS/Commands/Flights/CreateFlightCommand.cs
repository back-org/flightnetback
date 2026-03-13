using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Flights;

/// <summary>
/// Commande MediatR pour la création d'un vol.
/// </summary>
/// <param name="Dto">Données du vol à créer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record CreateFlightCommand(FlightDto Dto, string PerformedBy) : IRequest<FlightDto>;

/// <summary>
/// Handler pour la commande de création de vol.
/// </summary>
public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, FlightDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateFlightCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<FlightDto> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Flight.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Flight",
            entityId: entity.Id.ToString(),
            details: $"Vol créé: {entity.Code}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}