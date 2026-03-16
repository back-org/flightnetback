using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Users;

/// <summary>
/// Requête permettant de récupérer tous les utilisateurs.
/// </summary>
public record GetAllUsersQuery() : IRequest<IEnumerable<UserDto>>;

/// <summary>
/// Handler chargé de traiter la récupération de tous les utilisateurs.
/// </summary>
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IRepositoryManager _manager;

    /// <summary>
    /// Initialise le handler de récupération de tous les utilisateurs.
    /// </summary>
    public GetAllUsersQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.User.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}