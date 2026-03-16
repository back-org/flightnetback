/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Roles' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Roles;

/// <summary>
/// Requête permettant de récupérer un rôle par son identifiant.
/// </summary>
/// <param name="Id">Identifiant du rôle.</param>
public record GetRoleByIdQuery(int Id) : IRequest<RoleDto?>;

/// <summary>
/// Handler chargé de récupérer un rôle par son identifiant.
/// </summary>
public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRepositoryManager _manager;

    public GetRoleByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    /// <inheritdoc />
    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Role.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}