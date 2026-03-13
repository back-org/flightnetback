using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Bookings;

public record GetAllBookingsQuery : IRequest<IReadOnlyCollection<BookingDto>>;

public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, IReadOnlyCollection<BookingDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllBookingsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IReadOnlyCollection<BookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Booking.AllAsync();
        return entities.Select(x => x.ToDto()).ToList();
    }
}