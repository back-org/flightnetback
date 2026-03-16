using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Baggages;

/// <summary>
/// Commande permettant de créer un bagage.
/// </summary>
public record CreateBaggageCommand(BaggageDto Dto, string PerformedBy) : IRequest<BaggageDto>;

/// <summary>
/// Handler chargé de créer un bagage.
/// </summary>
public class CreateBaggageCommandHandler : IRequestHandler<CreateBaggageCommand, BaggageDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateBaggageCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<BaggageDto> Handle(CreateBaggageCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Baggage.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Baggage",
            entityId: entity.Id.ToString(),
            details: $"Bagage créé : {entity.TagNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}