using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public class ApplicationSettings
{
    [Required]
    public string Culture { get; set; }

    [Required]
    public string ConnectionString { get; set; }

    public IReadOnlyCollection<PriceRange> PriceDistributionRanges { get; set; }
}
#nullable restore warnings

public sealed class ApplicationSettingsValidation
    : IValidateOptions<ApplicationSettings>
{
    public ValidateOptionsResult Validate(string? name, ApplicationSettings options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.PriceDistributionRanges.Count <= 0)
        {
            return ValidateOptionsResult.Fail($"{nameof(ApplicationSettings.PriceDistributionRanges)} in appsettings is empty.");
        }
        return ValidateOptionsResult.Success;
    }
}
