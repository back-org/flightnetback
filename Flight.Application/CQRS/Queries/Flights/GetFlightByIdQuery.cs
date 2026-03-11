using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Flights;

/// <summary>
/// Requête MediatR pour récupérer un vol par son identifiant.
/// </summary>
public record GetFlightByIdQuery(int Id) : IRequest<FlightDto?>;

/// <summary>
/// Handler pour la récupération d'un vol par identifiant.
/// </summary>
public class GetFlightByIdQueryHandler : IRequestHandler<GetFlightByIdQuery, FlightDto?>
{
    private readonly IRepositoryManager _manager;

    public GetFlightByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<FlightDto?> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
    {
        var flight = await _manager.Flight.GetByIdAsync(request.Id);
        return flight?.ToDto();
    }
}
