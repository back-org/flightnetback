/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/SeatAssignements' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.SeatAssignments;

/// <summary>
/// Commande permettant de créer une attribution de siège.
/// </summary>
public record CreateSeatAssignmentCommand(SeatAssignmentDto Dto, string PerformedBy) : IRequest<SeatAssignmentDto>;

/// <summary>
/// Handler chargé de créer une attribution de siège.
/// </summary>
public class CreateSeatAssignmentCommandHandler : IRequestHandler<CreateSeatAssignmentCommand, SeatAssignmentDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateSeatAssignmentCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<SeatAssignmentDto> Handle(CreateSeatAssignmentCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.SeatAssignment.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "SeatAssignment",
            entityId: entity.Id.ToString(),
            details: $"Attribution siège créée : {entity.SeatNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}