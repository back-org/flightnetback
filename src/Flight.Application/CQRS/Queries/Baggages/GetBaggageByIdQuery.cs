/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Baggages' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Baggages;

/// <summary>
/// Requête permettant de récupérer un bagage par son identifiant.
/// </summary>
public record GetBaggageByIdQuery(int Id) : IRequest<BaggageDto?>;

/// <summary>
/// Handler chargé de récupérer un bagage par identifiant.
/// </summary>
public class GetBaggageByIdQueryHandler : IRequestHandler<GetBaggageByIdQuery, BaggageDto?>
{
    private readonly IRepositoryManager _manager;

    public GetBaggageByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<BaggageDto?> Handle(GetBaggageByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Baggage.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}