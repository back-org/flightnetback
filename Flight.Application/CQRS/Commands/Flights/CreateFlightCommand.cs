using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Flights;

/// <summary>
/// Commande MediatR pour la création d'un vol.
/// </summary>
public record CreateFlightCommand(FlightDto Dto) : IRequest<FlightDto>;

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
        // Conversion DTO → Entity
        var entity = request.Dto.ToEntity();

        await _manager.Flight.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Flight",
            entityId: entity.Id.ToString(),
            details: $"Vol créé: {entity.Code}",
            cancellationToken: cancellationToken);

        // Conversion Entity → DTO
        return entity.ToDto();
    }
}