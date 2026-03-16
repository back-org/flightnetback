using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Payments;

/// <summary>
/// Commande permettant de mettre à jour un paiement existant.
/// </summary>
public record UpdatePaymentCommand(int Id, PaymentDto Dto, string PerformedBy) : IRequest<PaymentDto?>;

/// <summary>
/// Handler chargé de mettre à jour un paiement.
/// </summary>
public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, PaymentDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdatePaymentCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<PaymentDto?> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Payment.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Payment.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Payment",
            entityId: existing.Id.ToString(),
            details: $"Paiement mis à jour pour réservation #{existing.BookingId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}