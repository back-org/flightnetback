using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Aircrafts;

/// <summary>
/// Requête permettant de récupérer un avion par son identifiant.
/// </summary>
public record GetAircraftByIdQuery(int Id) : IRequest<AircraftDto?>;

/// <summary>
/// Handler chargé de récupérer un avion par son identifiant.
/// </summary>
public class GetAircraftByIdQueryHandler : IRequestHandler<GetAircraftByIdQuery, AircraftDto?>
{
    private readonly IRepositoryManager _manager;

    public GetAircraftByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<AircraftDto?> Handle(GetAircraftByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Aircraft.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}