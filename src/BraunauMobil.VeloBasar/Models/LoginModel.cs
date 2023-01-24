using FluentValidation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models;

#nullable disable warnings
public sealed class LoginModel
{
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public bool ShowErrorMessage { get; set; }
}

public sealed class LoginModelValidator
    : AbstractValidator<LoginModel>
{
    public LoginModelValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(login => login.Email)
            .NotNull()
            .EmailAddress()
            .WithMessage(localizer[VeloTexts.PleaseEnterValidEMail]);

        RuleFor(login => login.Password)
            .NotNull();
    }
}
