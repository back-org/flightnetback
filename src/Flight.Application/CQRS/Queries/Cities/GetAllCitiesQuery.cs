/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Cities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

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