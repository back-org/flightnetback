using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Aircrafts;

/// <summary>
/// Commande permettant de créer un nouvel avion.
/// </summary>
public record CreateAircraftCommand(AircraftDto Dto, string PerformedBy) : IRequest<AircraftDto>;

/// <summary>
/// Handler chargé de créer un avion.
/// </summary>
public class CreateAircraftCommandHandler : IRequestHandler<CreateAircraftCommand, AircraftDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateAircraftCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<AircraftDto> Handle(CreateAircraftCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Aircraft.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Aircraft",
            entityId: entity.Id.ToString(),
            details: $"Avion créé : {entity.RegistrationNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}