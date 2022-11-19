using FluentValidation;
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
    private readonly VeloTexts _txt;

    public LoginModelValidator(VeloTexts txt)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(login => login.Email)
            .NotNull()
            .EmailAddress()
            .WithMessage(_txt.PleaseEnterValidEMail);

        RuleFor(login => login.Password)
            .NotNull();
    }
}
