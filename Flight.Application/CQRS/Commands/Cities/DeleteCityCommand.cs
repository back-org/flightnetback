using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Cities;

public record DeleteCityCommand(int Id, string? PerformedBy = null) : IRequest<bool>;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteCityCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.City.GetByIdAsync(request.Id);
        if (existing is null) return false;

        await _manager.City.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "City",
            entityId: request.Id.ToString(),
            details: $"Ville supprimée: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}