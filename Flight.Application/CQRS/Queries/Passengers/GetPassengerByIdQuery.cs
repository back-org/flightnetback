using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Passengers;

/// <summary>
/// Requête MediatR pour récupérer un passager par identifiant.
/// </summary>
/// <param name="Id">Identifiant du passager recherché.</param>
public record GetPassengerByIdQuery(int Id) : IRequest<PassengerDto?>;

/// <summary>
/// Handler chargé de récupérer un passager par identifiant.
/// </summary>
public class GetPassengerByIdQueryHandler : IRequestHandler<GetPassengerByIdQuery, PassengerDto?>
{
    private readonly IRepositoryManager _manager;

    public GetPassengerByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<PassengerDto?> Handle(GetPassengerByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _manager.Passenger.GetByIdAsync(request.Id);
        return item?.ToDto();
    }
}