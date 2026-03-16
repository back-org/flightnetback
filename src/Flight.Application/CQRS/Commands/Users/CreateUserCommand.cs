/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Users' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Users;

/// <summary>
/// Commande permettant de créer un nouvel utilisateur.
/// </summary>
/// <param name="Dto">DTO contenant les données de l'utilisateur à créer.</param>
/// <param name="PerformedBy">Nom de l'utilisateur ou système ayant lancé l'action.</param>
public record CreateUserCommand(UserDto Dto, string PerformedBy) : IRequest<UserDto>;

/// <summary>
/// Handler chargé de créer un nouvel utilisateur.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateUserCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    /// <inheritdoc />
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.User.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "User",
            entityId: entity.Id.ToString(),
            details: $"Utilisateur créé : {entity.UserName}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}