using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Cities;

public record GetAllCitiesQuery : IRequest<IReadOnlyCollection<CityDto>>;

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IReadOnlyCollection<CityDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllCitiesQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IReadOnlyCollection<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.City.AllAsync();
        return entities.Select(x => x.ToDto()).ToList();
    }
}