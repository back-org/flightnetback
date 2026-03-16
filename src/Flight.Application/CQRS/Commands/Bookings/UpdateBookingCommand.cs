/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Bookings' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Bookings;

public record UpdateBookingCommand(int Id, BookingDto Dto, string? PerformedBy = null) : IRequest<BookingDto?>;

public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, BookingDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateBookingCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<BookingDto?> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Booking.GetByIdAsync(request.Id);
        if (existing is null) return null;

        existing.UpdateEntity(request.Dto);
        await _manager.Booking.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Booking",
            entityId: existing.Id.ToString(),
            details: $"Réservation mise à jour: vol {existing.FlightId}, passager {existing.PassengerId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}