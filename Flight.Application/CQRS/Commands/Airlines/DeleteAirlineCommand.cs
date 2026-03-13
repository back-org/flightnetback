using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Airlines;

/// <summary>
/// Commande MediatR pour supprimer une compagnie aérienne.
/// </summary>
public record DeleteAirlineCommand(int Id, string? PerformedBy = null) : IRequest<bool>;

/// <summary>
/// Handler pour la suppression d'une compagnie aérienne.
/// </summary>
public class DeleteAirlineCommandHandler : IRequestHandler<DeleteAirlineCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteAirlineCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteAirlineCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Airline.GetByIdAsync(request.Id);
        if (existing is null) return false;

        await _manager.Airline.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Airline",
            entityId: request.Id.ToString(),
            details: $"Compagnie supprimée: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}