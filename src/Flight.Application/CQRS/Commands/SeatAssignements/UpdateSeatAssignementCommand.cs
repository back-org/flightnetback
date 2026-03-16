using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.SeatAssignments;

/// <summary>
/// Commande permettant de mettre à jour une attribution de siège existante.
/// </summary>
public record UpdateSeatAssignmentCommand(int Id, SeatAssignmentDto Dto, string PerformedBy) : IRequest<SeatAssignmentDto?>;

/// <summary>
/// Handler chargé de mettre à jour une attribution de siège.
/// </summary>
public class UpdateSeatAssignmentCommandHandler : IRequestHandler<UpdateSeatAssignmentCommand, SeatAssignmentDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateSeatAssignmentCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<SeatAssignmentDto?> Handle(UpdateSeatAssignmentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.SeatAssignment.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.SeatAssignment.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "SeatAssignment",
            entityId: existing.Id.ToString(),
            details: $"Attribution siège mise à jour : {existing.SeatNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}