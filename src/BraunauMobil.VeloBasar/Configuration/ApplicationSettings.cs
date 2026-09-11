using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public class ApplicationSettings
{
    [Required]
    public string Culture { get; set; }

    [Required]
    public string ConnectionString { get; set; }

    public IReadOnlyCollection<PriceRange> PriceDistributionRanges { get; set; }

    public IReadOnlyCollection<string> Colors { get; set; }
}
#nullable restore warnings

public sealed class ApplicationSettingsValidation
    : IValidateOptions<ApplicationSettings>
{
    public ValidateOptionsResult Validate(string? name, ApplicationSettings options)
    {
        ArgumentNullException.ThrowIfNull(options);

        List<string> failures = [];
        if (options.PriceDistributionRanges is null)
        {
            failures.Add($"{nameof(ApplicationSettings.PriceDistributionRanges)} in appsettings are not set.");
        }
        else if (options.PriceDistributionRanges.Count <= 0)
        {
            failures.Add($"{nameof(ApplicationSettings.PriceDistributionRanges)} in appsettings is empty.");
        }

        if (options.Colors is null)
        {
            failures.Add($"{nameof(ApplicationSettings.Colors)} in appsettings are not set.");
        }
        else if (options.Colors.Count <= 0)
        {
            failures.Add($"{nameof(ApplicationSettings.Colors)} in appsettings is empty.");
        }
        else
        {
            foreach (string colorString in options.Colors)
            {
                if (!Color.TryParse(colorString, out Color color))
                {
                    failures.Add($"Color in appsettings {colorString} cannot be parsed.");
                }
            }
        }

        if (failures.Count > 0)
        {
            return ValidateOptionsResult.Fail(failures);
        }

        return ValidateOptionsResult.Success;
    }
}
