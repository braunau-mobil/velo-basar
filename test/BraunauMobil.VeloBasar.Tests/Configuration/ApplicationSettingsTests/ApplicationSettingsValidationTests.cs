using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.Configuration.ApplicationSettingsTests;

public class ApplicationSettingsValidationTests
{
    private static readonly string[] _validColors = ["#007bff", "#6c757d"];

    private readonly ApplicationSettingsValidation _sut = new();

    [Theory]
    [VeloAutoData]
    public void PriceDistributionRangesEmpty_ShouldFail(ApplicationSettings settings, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = Array.Empty<PriceRange>();
        settings.Colors = _validColors;

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Failed.Should().BeTrue();
    }

    [Theory]
    [VeloAutoData]
    public void PriceDistributionRangesNull_ShouldFail(ApplicationSettings settings, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = null!;
        settings.Colors = _validColors;

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Failed.Should().BeTrue();
    }

    [Theory]
    [VeloAutoData]
    public void ColorsEmpty_ShouldFail(ApplicationSettings settings, PriceRange[] priceRanges, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = priceRanges;
        settings.Colors = Array.Empty<string>();

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Failed.Should().BeTrue();
    }

    [Theory]
    [VeloAutoData]
    public void ColorsNull_ShouldFail(ApplicationSettings settings, PriceRange[] priceRanges, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = priceRanges;
        settings.Colors = null!;

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Failed.Should().BeTrue();
    }

    [Theory]
    [VeloAutoData]
    public void ColorsContainsUnparsableColor_ShouldFail(ApplicationSettings settings, PriceRange[] priceRanges, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = priceRanges;
        settings.Colors = ["#007bff", "not-a-color"];

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Failed.Should().BeTrue();
        validation.Failures.Should().Contain(failure => failure.Contains("not-a-color"));
    }

    [Theory]
    [VeloAutoData]
    public void NoEmpty_ShouldSucceed(ApplicationSettings settings, PriceRange[] priceRanges, string? name)
    {
        //  Arrange
        settings.PriceDistributionRanges = priceRanges;
        settings.Colors = _validColors;

        //  Act
        ValidateOptionsResult validation = _sut.Validate(name, settings);

        //  Assert
        validation.Succeeded.Should().BeTrue();
    }
}
