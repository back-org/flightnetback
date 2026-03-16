/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Aircrafts' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Aircrafts;

/// <summary>
/// Requête permettant de récupérer tous les avions.
/// </summary>
public record GetAllAircraftsQuery() : IRequest<IEnumerable<AircraftDto>>;

/// <summary>
/// Handler chargé de récupérer tous les avions.
/// </summary>
public class GetAllAircraftsQueryHandler : IRequestHandler<GetAllAircraftsQuery, IEnumerable<AircraftDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllAircraftsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<AircraftDto>> Handle(GetAllAircraftsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Aircraft.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}