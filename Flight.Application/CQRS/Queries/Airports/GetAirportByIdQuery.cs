using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Airports;

public record GetAirportByIdQuery(int Id) : IRequest<AirportDto?>;

public class GetAirportByIdQueryHandler : IRequestHandler<GetAirportByIdQuery, AirportDto?>
{
    private readonly IRepositoryManager _manager;

    public GetAirportByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<AirportDto?> Handle(GetAirportByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Airport.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}