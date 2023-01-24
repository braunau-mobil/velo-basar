using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class CountryEntity
    : AbstractCrudEntity
{
    public string Name { get; set; }

    public string Iso3166Alpha3Code { get; set; }
}

public sealed class CountryEntityValidator
    : AbstractValidator<CountryEntity>
{
    public CountryEntityValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterName]);

        RuleFor(b => b.Iso3166Alpha3Code)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterIso3166Alpha3Code]);
    }
}
