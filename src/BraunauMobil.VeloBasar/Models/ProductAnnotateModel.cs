using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Models;

public sealed class ProductAnnotateModel
{
    public int ProductId { get; set; }

    public string Notes { get; set; } = "";
}

public sealed class ProductAnnotateModelValidator
    : AbstractValidator<ProductAnnotateModel>
{
    public ProductAnnotateModelValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(x => x.Notes)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterNotes]);
    }
}
