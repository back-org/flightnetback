using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Airlines;

/// <summary>
/// Requête MediatR pour récupérer toutes les compagnies aériennes.
/// </summary>
public record GetAllAirlinesQuery : IRequest<IReadOnlyCollection<AirlineDto>>;

/// <summary>
/// Handler pour la récupération de toutes les compagnies aériennes.
/// </summary>
public class GetAllAirlinesQueryHandler : IRequestHandler<GetAllAirlinesQuery, IReadOnlyCollection<AirlineDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllAirlinesQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IReadOnlyCollection<AirlineDto>> Handle(GetAllAirlinesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Airline.AllAsync();
        return entities.Select(x => x.ToDto()).ToList();
    }
}