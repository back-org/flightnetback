/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/CrewMembers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.CrewMembers;

/// <summary>
/// Requête permettant de récupérer tous les membres d'équipe.
/// </summary>
public record GetAllCrewMembersQuery() : IRequest<IEnumerable<CrewMemberDto>>;

/// <summary>
/// Handler chargé de récupérer tous les membres d'équipe.
/// </summary>
public class GetAllCrewMembersQueryHandler : IRequestHandler<GetAllCrewMembersQuery, IEnumerable<CrewMemberDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllCrewMembersQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<CrewMemberDto>> Handle(GetAllCrewMembersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.CrewMember.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}