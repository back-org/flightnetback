/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Cities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Cities;

public record GetCityByIdQuery(int Id) : IRequest<CityDto?>;

public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto?>
{
    private readonly IRepositoryManager _manager;

    public GetCityByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<CityDto?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.City.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}