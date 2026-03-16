using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Payments;

/// <summary>
/// Requête permettant de récupérer tous les paiements.
/// </summary>
public record GetAllPaymentsQuery() : IRequest<IEnumerable<PaymentDto>>;

/// <summary>
/// Handler chargé de récupérer tous les paiements.
/// </summary>
public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllPaymentsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Payment.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}