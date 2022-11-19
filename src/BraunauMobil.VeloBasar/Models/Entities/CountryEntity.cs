using FluentValidation;

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
    public CountryEntityValidator(VeloTexts txt)
    {
        ArgumentNullException.ThrowIfNull(txt);

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(txt.PleaseEnterName);

        RuleFor(b => b.Iso3166Alpha3Code)
            .NotEmpty()
            .WithMessage(txt.PleaseEnterIso3166Alpha3Code);
    }
}
