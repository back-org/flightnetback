using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Airports;

public record GetAllAirportsQuery : IRequest<IReadOnlyCollection<AirportDto>>;

public class GetAllAirportsQueryHandler : IRequestHandler<GetAllAirportsQuery, IReadOnlyCollection<AirportDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllAirportsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IReadOnlyCollection<AirportDto>> Handle(GetAllAirportsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Airport.AllAsync();
        return entities.Select(x => x.ToDto()).ToList();
    }
}