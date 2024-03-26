using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.Configuration.ApplicationSettingsTests;

public class ApplicationSettingsValidationTests
{
    private readonly ApplicationSettingsValidation _sut = new();

    [Theory]
    [VeloAutoData]
    public void Empty_ShouldFail(ApplicationSettings settings, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = Array.Empty<PriceRange>();

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Failed.Should().BeTrue();
    }

    [Theory]
    [VeloAutoData]
    public void NoEmpty_ShouldSucceed(ApplicationSettings settings, PriceRange[] priceRanges, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = priceRanges;

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Succeeded.Should().BeTrue();
    }
}
