/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Passengers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Passengers;

/// <summary>
/// Requête MediatR pour récupérer tous les passagers.
/// </summary>
public record GetAllPassengersQuery() : IRequest<IEnumerable<PassengerDto>>;

/// <summary>
/// Handler chargé de récupérer tous les passagers.
/// </summary>
public class GetAllPassengersQueryHandler : IRequestHandler<GetAllPassengersQuery, IEnumerable<PassengerDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllPassengersQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<PassengerDto>> Handle(GetAllPassengersQuery request, CancellationToken cancellationToken)
    {
        var items = await _manager.Passenger.AllAsync();
        return items.Select(x => x.ToDto());
    }
}