using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Flights;

/// <summary>
/// Commande MediatR pour la suppression d'un vol.
/// </summary>
public record DeleteFlightCommand(int Id) : IRequest<bool>;

public class DeleteFlightCommandHandler : IRequestHandler<DeleteFlightCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteFlightCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteFlightCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Flight.GetByIdAsync(request.Id);
        if (existing is null) return false;

        await _manager.Flight.Delete(existing);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Flight",
            entityId: request.Id.ToString(),
            details: $"Vol supprimé: {existing.Code}",
            cancellationToken: cancellationToken);

        return true;
    }
}
