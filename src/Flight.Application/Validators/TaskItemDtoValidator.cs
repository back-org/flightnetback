using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO tâche.
/// </summary>
public class TaskItemDtoValidator : AbstractValidator<TaskItemDto>
{
    public TaskItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Le titre de la tâche est requis.")
            .MaximumLength(100).WithMessage("Le titre de la tâche ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.CreatedByUserId)
            .GreaterThan(0).WithMessage("L'identifiant du créateur doit être supérieur à zéro.");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("La priorité est requise.")
            .MaximumLength(20).WithMessage("La priorité ne peut pas dépasser 20 caractères.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le statut de la tâche est requis.")
            .MaximumLength(30).WithMessage("Le statut de la tâche ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("La date de création ne peut pas être dans le futur.");

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(x => x.CreatedAt)
            .When(x => x.DueDate.HasValue)
            .WithMessage("La date limite ne peut pas être antérieure à la date de création.");
    }
}