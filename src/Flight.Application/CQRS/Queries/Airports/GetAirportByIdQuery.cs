/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Airports' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Airports;

public record GetAirportByIdQuery(int Id) : IRequest<AirportDto?>;

public class GetAirportByIdQueryHandler : IRequestHandler<GetAirportByIdQuery, AirportDto?>
{
    private readonly IRepositoryManager _manager;

    public GetAirportByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<AirportDto?> Handle(GetAirportByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Airport.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}