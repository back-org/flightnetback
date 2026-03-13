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