/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Bookings' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Bookings;

public record GetBookingByIdQuery(int Id) : IRequest<BookingDto?>;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto?>
{
    private readonly IRepositoryManager _manager;

    public GetBookingByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<BookingDto?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Booking.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}