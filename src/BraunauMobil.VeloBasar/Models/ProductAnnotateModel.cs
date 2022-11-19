using FluentValidation;

namespace BraunauMobil.VeloBasar.Models;

public sealed class ProductAnnotateModel
{
    public int ProductId { get; set; }

    public string Notes { get; set; } = "";
}

public sealed class ProductAnnotateModelValidator
    : AbstractValidator<ProductAnnotateModel>
{
    public ProductAnnotateModelValidator(VeloTexts txt)
    {
        ArgumentNullException.ThrowIfNull(txt);

        RuleFor(x => x.Notes)
            .NotEmpty()
            .WithMessage(txt.PleaseEnterNotes);
    }
}
