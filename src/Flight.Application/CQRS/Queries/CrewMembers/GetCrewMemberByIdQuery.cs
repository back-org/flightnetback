/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/CrewMembers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.CrewMembers;

/// <summary>
/// Requête permettant de récupérer un membre d'équipe par son identifiant.
/// </summary>
public record GetCrewMemberByIdQuery(int Id) : IRequest<CrewMemberDto?>;

/// <summary>
/// Handler chargé de récupérer un membre d'équipe par identifiant.
/// </summary>
public class GetCrewMemberByIdQueryHandler : IRequestHandler<GetCrewMemberByIdQuery, CrewMemberDto?>
{
    private readonly IRepositoryManager _manager;

    public GetCrewMemberByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<CrewMemberDto?> Handle(GetCrewMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.CrewMember.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}