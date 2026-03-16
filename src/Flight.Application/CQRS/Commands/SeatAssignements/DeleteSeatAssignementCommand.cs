using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.SeatAssignments;

/// <summary>
/// Commande permettant de supprimer une attribution de siège.
/// </summary>
public record DeleteSeatAssignmentCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer une attribution de siège.
/// </summary>
public class DeleteSeatAssignmentCommandHandler : IRequestHandler<DeleteSeatAssignmentCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteSeatAssignmentCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteSeatAssignmentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.SeatAssignment.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.SeatAssignment.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "SeatAssignment",
            entityId: request.Id.ToString(),
            details: $"Attribution siège supprimée : {existing.SeatNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}