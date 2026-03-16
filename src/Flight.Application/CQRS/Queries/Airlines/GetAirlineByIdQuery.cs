/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Airlines' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Airlines;

/// <summary>
/// Requête MediatR pour récupérer une compagnie aérienne par identifiant.
/// </summary>
public record GetAirlineByIdQuery(int Id) : IRequest<AirlineDto?>;

/// <summary>
/// Handler pour la récupération d'une compagnie aérienne par identifiant.
/// </summary>
public class GetAirlineByIdQueryHandler : IRequestHandler<GetAirlineByIdQuery, AirlineDto?>
{
    private readonly IRepositoryManager _manager;

    public GetAirlineByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<AirlineDto?> Handle(GetAirlineByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Airline.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}