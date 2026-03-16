/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Tickets' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Tickets;

/// <summary>
/// Commande permettant de mettre à jour un billet existant.
/// </summary>
public record UpdateTicketCommand(int Id, TicketDto Dto, string PerformedBy) : IRequest<TicketDto?>;

/// <summary>
/// Handler chargé de mettre à jour un billet.
/// </summary>
public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, TicketDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateTicketCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<TicketDto?> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Ticket.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Ticket.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Ticket",
            entityId: existing.Id.ToString(),
            details: $"Billet mis à jour : {existing.TicketNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}