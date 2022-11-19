using FluentValidation;

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
    public BrandEntityValidator(VeloTexts txt)
    {
        ArgumentNullException.ThrowIfNull(txt);

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(txt.PleaseEnterName);
    }
}
