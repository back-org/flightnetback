using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Cities;

public record UpdateCityCommand(int Id, CityDto Dto, string? PerformedBy = null) : IRequest<CityDto?>;

public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, CityDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateCityCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<CityDto?> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.City.GetByIdAsync(request.Id);
        if (existing is null) return null;

        existing.UpdateEntity(request.Dto);
        await _manager.City.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "City",
            entityId: existing.Id.ToString(),
            details: $"Ville mise à jour: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}