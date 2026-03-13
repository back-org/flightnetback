using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Countries;

/// <summary>
/// Commande MediatR pour la mise à jour d'un pays.
/// </summary>
/// <param name="Id">Identifiant du pays à mettre à jour.</param>
/// <param name="Dto">Données mises à jour.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record UpdateCountryCommand(int Id, CountryDto Dto, string PerformedBy) : IRequest<CountryDto?>;

/// <summary>
/// Handler chargé de traiter la mise à jour d'un pays.
/// </summary>
public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, CountryDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateCountryCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<CountryDto?> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Country.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Country.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Country",
            entityId: existing.Id.ToString(),
            details: $"Pays mis à jour: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}