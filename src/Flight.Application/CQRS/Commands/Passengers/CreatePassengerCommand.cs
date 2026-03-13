using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Passengers;

/// <summary>
/// Commande MediatR pour la création d'un passager.
/// </summary>
/// <param name="Dto">Données du passager à créer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record CreatePassengerCommand(PassengerDto Dto, string PerformedBy) : IRequest<PassengerDto>;

/// <summary>
/// Handler chargé de traiter la création d'un passager.
/// </summary>
public class CreatePassengerCommandHandler : IRequestHandler<CreatePassengerCommand, PassengerDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreatePassengerCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<PassengerDto> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Passenger.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Passenger",
            entityId: entity.Id.ToString(),
            details: $"Passager créé: {entity.Name} {entity.LastName}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}