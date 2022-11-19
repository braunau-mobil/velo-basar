using FluentValidation;

namespace BraunauMobil.VeloBasar.Models;

#nullable disable warnings
public class InitializationConfiguration
{
    public string AdminUserEMail { get; set; }
    
    public bool GenerateCountries { get; set; }
    
    public bool GenerateBrands { get; set; }
    
    public bool GenerateProductTypes { get; set; }
    
    public bool GenerateZipCodes { get; set; }
}

public sealed class InitializationConfigurationValidator
    : AbstractValidator<InitializationConfiguration>
{
    private readonly VeloTexts _txt;

    public InitializationConfigurationValidator(VeloTexts txt)
    {
        _txt = txt ?? throw new ArgumentNullException(nameof(txt));

        RuleFor(seller => seller.AdminUserEMail)
            .NotNull()
            .WithMessage(_txt.PleaseEnterAdminUserEmail);
    }
}
