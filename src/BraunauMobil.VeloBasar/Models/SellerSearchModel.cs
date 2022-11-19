using FluentValidation;

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
    private readonly VeloTexts _txt;

    public SellerSearchModelValidator(VeloTexts txt)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(seller => seller.FirstName)
            .NotNull()
            .WithMessage(_txt.PleaseEnterFirstNameForSearch);

        RuleFor(seller => seller.LastName)
            .NotNull()
            .WithMessage(_txt.PleaseEnterLastNameForSearch);
    }
}
