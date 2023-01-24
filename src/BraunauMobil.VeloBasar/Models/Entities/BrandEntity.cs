using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class BrandEntity
    : AbstractCrudEntity
{
    public string Name { get; set; }
}

public sealed class BrandEntityValidator
    : AbstractValidator<BrandEntity>
{
    public BrandEntityValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterName]);
    }
}
