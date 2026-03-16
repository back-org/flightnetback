/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Payements' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Payments;

/// <summary>
/// Requête permettant de récupérer un paiement par son identifiant.
/// </summary>
public record GetPaymentByIdQuery(int Id) : IRequest<PaymentDto?>;

/// <summary>
/// Handler chargé de récupérer un paiement par identifiant.
/// </summary>
public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto?>
{
    private readonly IRepositoryManager _manager;

    public GetPaymentByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<PaymentDto?> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Payment.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}