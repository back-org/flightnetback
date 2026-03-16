using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Baggages;

/// <summary>
/// Commande permettant de supprimer un bagage.
/// </summary>
public record DeleteBaggageCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer un bagage.
/// </summary>
public class DeleteBaggageCommandHandler : IRequestHandler<DeleteBaggageCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteBaggageCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteBaggageCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Baggage.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.Baggage.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Baggage",
            entityId: request.Id.ToString(),
            details: $"Bagage supprimé : {existing.TagNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}