/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Passengers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Passengers;

/// <summary>
/// Requête MediatR pour récupérer un passager par identifiant.
/// </summary>
/// <param name="Id">Identifiant du passager recherché.</param>
public record GetPassengerByIdQuery(int Id) : IRequest<PassengerDto?>;

/// <summary>
/// Handler chargé de récupérer un passager par identifiant.
/// </summary>
public class GetPassengerByIdQueryHandler : IRequestHandler<GetPassengerByIdQuery, PassengerDto?>
{
    private readonly IRepositoryManager _manager;

    public GetPassengerByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<PassengerDto?> Handle(GetPassengerByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _manager.Passenger.GetByIdAsync(request.Id);
        return item?.ToDto();
    }
}