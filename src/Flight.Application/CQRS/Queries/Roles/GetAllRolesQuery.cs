/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Roles' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Roles;

/// <summary>
/// Requête permettant de récupérer tous les rôles.
/// </summary>
public record GetAllRolesQuery() : IRequest<IEnumerable<RoleDto>>;

/// <summary>
/// Handler chargé de récupérer tous les rôles.
/// </summary>
public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllRolesQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Role.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}