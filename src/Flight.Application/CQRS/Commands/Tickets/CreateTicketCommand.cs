/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Tickets' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Tickets;

/// <summary>
/// Commande permettant de créer un billet.
/// </summary>
public record CreateTicketCommand(TicketDto Dto, string PerformedBy) : IRequest<TicketDto>;

/// <summary>
/// Handler chargé de créer un billet.
/// </summary>
public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateTicketCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Ticket.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Ticket",
            entityId: entity.Id.ToString(),
            details: $"Billet créé : {entity.TicketNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}