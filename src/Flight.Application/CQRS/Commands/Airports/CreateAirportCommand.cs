/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Airports' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Airports;

public record CreateAirportCommand(AirportDto Dto, string? PerformedBy = null) : IRequest<AirportDto>;

public class CreateAirportCommandHandler : IRequestHandler<CreateAirportCommand, AirportDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateAirportCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<AirportDto> Handle(CreateAirportCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();
        await _manager.Airport.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Airport",
            entityId: entity.Id.ToString(),
            details: $"Aéroport créé: {entity.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}