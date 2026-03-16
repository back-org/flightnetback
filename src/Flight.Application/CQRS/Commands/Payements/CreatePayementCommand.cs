/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Payements' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Payments;

/// <summary>
/// Commande permettant de créer un paiement.
/// </summary>
public record CreatePaymentCommand(PaymentDto Dto, string PerformedBy) : IRequest<PaymentDto>;

/// <summary>
/// Handler chargé de créer un paiement.
/// </summary>
public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreatePaymentCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Payment.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Payment",
            entityId: entity.Id.ToString(),
            details: $"Paiement créé pour réservation #{entity.BookingId}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}