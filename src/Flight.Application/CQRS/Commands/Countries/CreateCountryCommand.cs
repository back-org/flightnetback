/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Countries' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Countries;

/// <summary>
/// Commande MediatR pour la création d'un pays.
/// </summary>
/// <param name="Dto">Données du pays à créer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record CreateCountryCommand(CountryDto Dto, string PerformedBy) : IRequest<CountryDto>;

/// <summary>
/// Handler chargé de traiter la création d'un pays.
/// </summary>
public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, CountryDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateCountryCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<CountryDto> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Country.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Country",
            entityId: entity.Id.ToString(),
            details: $"Pays créé: {entity.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}