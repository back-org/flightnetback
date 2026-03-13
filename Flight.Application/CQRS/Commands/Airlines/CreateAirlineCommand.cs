using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Airlines;

/// <summary>
/// Commande MediatR pour créer une compagnie aérienne.
/// </summary>
public record CreateAirlineCommand(AirlineDto Dto, string? PerformedBy = null) : IRequest<AirlineDto>;

/// <summary>
/// Handler pour la création d'une compagnie aérienne.
/// </summary>
public class CreateAirlineCommandHandler : IRequestHandler<CreateAirlineCommand, AirlineDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateAirlineCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<AirlineDto> Handle(CreateAirlineCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();
        await _manager.Airline.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Airline",
            entityId: entity.Id.ToString(),
            details: $"Compagnie créée: {entity.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}