using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Countries;

/// <summary>
/// Requête MediatR pour récupérer tous les pays.
/// </summary>
public record GetAllCountriesQuery() : IRequest<IEnumerable<CountryDto>>;

/// <summary>
/// Handler chargé de récupérer tous les pays.
/// </summary>
public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllCountriesQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        var items = await _manager.Country.AllAsync();
        return items.Select(x => x.ToDto());
    }
}