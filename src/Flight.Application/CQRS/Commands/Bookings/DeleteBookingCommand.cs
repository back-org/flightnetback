/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Bookings' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Bookings;

public record DeleteBookingCommand(int Id, string? PerformedBy = null) : IRequest<bool>;

public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteBookingCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Booking.GetByIdAsync(request.Id);
        if (existing is null) return false;

        await _manager.Booking.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Booking",
            entityId: request.Id.ToString(),
            details: $"Réservation supprimée: vol {existing.FlightId}, passager {existing.PassengerId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}