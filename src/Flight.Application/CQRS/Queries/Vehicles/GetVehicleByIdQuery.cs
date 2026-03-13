using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Vehicles;

/// <summary>
/// Requête MediatR pour récupérer un véhicule par identifiant.
/// </summary>
/// <param name="Id">Identifiant du véhicule recherché.</param>
public record GetVehicleByIdQuery(int Id) : IRequest<VehicleDto?>;

/// <summary>
/// Handler chargé de récupérer un véhicule par identifiant.
/// </summary>
public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    private readonly IRepositoryManager _manager;

    public GetVehicleByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _manager.Vehicle.GetByIdAsync(request.Id);
        return item?.ToDto();
    }
}