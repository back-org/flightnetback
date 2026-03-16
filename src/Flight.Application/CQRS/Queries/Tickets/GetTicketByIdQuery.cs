/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Tickets' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Tickets;

/// <summary>
/// Requête permettant de récupérer un billet par son identifiant.
/// </summary>
public record GetTicketByIdQuery(int Id) : IRequest<TicketDto?>;

/// <summary>
/// Handler chargé de récupérer un billet par identifiant.
/// </summary>
public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDto?>
{
    private readonly IRepositoryManager _manager;

    public GetTicketByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<TicketDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _manager.Ticket.GetByIdAsync(request.Id);
        return entity?.ToDto();
    }
}