
using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Bookings;

/// <summary>
/// Commande MediatR pour la création d'une réservation.
/// </summary>
public record CreateBookingCommand(BookingDto Dto, string PerformedBy) : IRequest<BookingDto>;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateBookingCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var entity = new Booking(request.Dto);
        await _manager.Booking.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Booking",
            entityId: entity.Id.ToString(),
            details: $"Réservation créée: vol {entity.FlightId}, passager {entity.PassengerId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}
