using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.SeatAssignments;

/// <summary>
/// Requête permettant de récupérer une attribution de siège par son identifiant.
/// </summary>
public record GetSeatAssignmentByIdQuery(int Id) : IRequest<SeatAssignmentDto?>;

/// <summary>
/// Handler chargé de récupérer une attribution de siège par identifiant.
/// </summary>
public class GetSeatAssignmentByIdQueryHandler : IRequestHandler<GetSeatAssignmentByIdQuery, SeatAssignmentDto?>
{
    private readonly IRepositoryManager _manager;

    public GetSeatAssignmentByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<SeatAssignmentDto?> Handle(GetSeatAssignmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.SeatAssignment.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}