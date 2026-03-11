using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Flights;

/// <summary>
/// Requête MediatR pour récupérer tous les vols.
/// </summary>
public record GetAllFlightsQuery : IRequest<IEnumerable<FlightDto>>;

/// <summary>
/// Handler pour la récupération de tous les vols.
/// </summary>
public class GetAllFlightsQueryHandler : IRequestHandler<GetAllFlightsQuery, IEnumerable<FlightDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllFlightsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<FlightDto>> Handle(GetAllFlightsQuery request, CancellationToken cancellationToken)
    {
        var flights = await _manager.Flight.AllAsync();
        return flights.Select(f => f.ToDto());
    }
}
