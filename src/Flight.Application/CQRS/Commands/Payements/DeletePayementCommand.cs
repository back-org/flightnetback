/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Payements' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Payments;

/// <summary>
/// Commande permettant de supprimer un paiement.
/// </summary>
public record DeletePaymentCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer un paiement.
/// </summary>
public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeletePaymentCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Payment.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.Payment.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Payment",
            entityId: request.Id.ToString(),
            details: $"Paiement supprimé pour réservation #{existing.BookingId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}