using FluentValidation;

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
    public ProductTypeEntityValidator(VeloTexts txt)
    {
        ArgumentNullException.ThrowIfNull(txt);

        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage(txt.PleaseEnterName);
    }
}