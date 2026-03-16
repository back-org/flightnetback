/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Users' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Users;

/// <summary>
/// Requête permettant de récupérer un utilisateur par son identifiant.
/// </summary>
/// <param name="Id">Identifiant de l'utilisateur recherché.</param>
public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;

/// <summary>
/// Handler chargé de récupérer un utilisateur par son identifiant.
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IRepositoryManager _manager;

    public GetUserByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    /// <inheritdoc />
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.User.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}