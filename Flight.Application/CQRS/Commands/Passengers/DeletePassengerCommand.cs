using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Passengers;

/// <summary>
/// Commande MediatR pour la suppression d'un passager.
/// </summary>
/// <param name="Id">Identifiant du passager à supprimer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record DeletePassengerCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de traiter la suppression d'un passager.
/// </summary>
public class DeletePassengerCommandHandler : IRequestHandler<DeletePassengerCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeletePassengerCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Passenger.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return false;
        }

        await _manager.Passenger.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Passenger",
            entityId: request.Id.ToString(),
            details: $"Passager supprimé: {existing.Name} {existing.LastName}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}