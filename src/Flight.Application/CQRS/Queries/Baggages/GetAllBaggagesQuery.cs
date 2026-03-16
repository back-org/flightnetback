/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Baggages' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Baggages;

/// <summary>
/// Requête permettant de récupérer tous les bagages.
/// </summary>
public record GetAllBaggagesQuery() : IRequest<IEnumerable<BaggageDto>>;

/// <summary>
/// Handler chargé de récupérer tous les bagages.
/// </summary>
public class GetAllBaggagesQueryHandler : IRequestHandler<GetAllBaggagesQuery, IEnumerable<BaggageDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllBaggagesQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<BaggageDto>> Handle(GetAllBaggagesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Baggage.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}