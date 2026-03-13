using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Vehicles;

/// <summary>
/// Requête MediatR pour récupérer tous les véhicules.
/// </summary>
public record GetAllVehiclesQuery() : IRequest<IEnumerable<VehicleDto>>;

/// <summary>
/// Handler chargé de récupérer tous les véhicules.
/// </summary>
public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllVehiclesQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var items = await _manager.Vehicle.AllAsync();
        return items.Select(x => x.ToDto());
    }
}