using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Models;

#nullable disable warnings
public sealed class SellerSearchModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}

public sealed class SellerSearchModelValidator
    : AbstractValidator<SellerSearchModel>
{
    public SellerSearchModelValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(seller => seller.FirstName)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterFirstNameForSearch]);

        RuleFor(seller => seller.LastName)
            .NotEmpty()
            .WithMessage(localizer[VeloTexts.PleaseEnterLastNameForSearch]);
    }
}
