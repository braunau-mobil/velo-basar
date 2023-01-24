using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class ProductTypeEntity
    : AbstractCrudEntity
{
    public string Name { get; set; }

    public string? Description { get; set; }
}

public sealed class ProductTypeEntityValidator
    : AbstractValidator<ProductTypeEntity>
{
    public ProductTypeEntityValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterName]);
    }
}