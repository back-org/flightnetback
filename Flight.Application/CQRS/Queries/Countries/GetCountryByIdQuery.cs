using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Countries;

/// <summary>
/// Requête MediatR pour récupérer un pays par identifiant.
/// </summary>
/// <param name="Id">Identifiant du pays recherché.</param>
public record GetCountryByIdQuery(int Id) : IRequest<CountryDto?>;

/// <summary>
/// Handler chargé de récupérer un pays par identifiant.
/// </summary>
public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, CountryDto?>
{
    private readonly IRepositoryManager _manager;

    public GetCountryByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<CountryDto?> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _manager.Country.GetByIdAsync(request.Id);
        return item?.ToDto();
    }
}