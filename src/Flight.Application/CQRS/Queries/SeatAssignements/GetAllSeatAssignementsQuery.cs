/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/SeatAssignements' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.SeatAssignments;

/// <summary>
/// Requête permettant de récupérer toutes les attributions de sièges.
/// </summary>
public record GetAllSeatAssignmentsQuery() : IRequest<IEnumerable<SeatAssignmentDto>>;

/// <summary>
/// Handler chargé de récupérer toutes les attributions de sièges.
/// </summary>
public class GetAllSeatAssignmentsQueryHandler : IRequestHandler<GetAllSeatAssignmentsQuery, IEnumerable<SeatAssignmentDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllSeatAssignmentsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<SeatAssignmentDto>> Handle(GetAllSeatAssignmentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.SeatAssignment.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}