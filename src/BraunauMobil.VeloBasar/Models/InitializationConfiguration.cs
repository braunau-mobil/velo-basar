using FluentValidation;
using Microsoft.Extensions.Localization;

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
    public InitializationConfigurationValidator(IStringLocalizer<SharedResources> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        RuleFor(seller => seller.AdminUserEMail)
            .NotNull()
            .WithMessage(localizer[VeloTexts.PleaseEnterAdminUserEmail]);
    }
}
