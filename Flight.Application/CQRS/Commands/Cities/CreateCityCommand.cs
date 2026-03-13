using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Cities;

public record CreateCityCommand(CityDto Dto, string? PerformedBy = null) : IRequest<CityDto>;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CityDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateCityCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<CityDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();
        await _manager.City.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "City",
            entityId: entity.Id.ToString(),
            details: $"Ville créée: {entity.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}