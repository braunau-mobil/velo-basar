using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.Configuration.PrintSettingsTests;

public class PrintSettingsValidationTests
{
    private readonly PrintSettingsValidation _sut = new();

    [Theory]
    [VeloAutoData]
    public void UseBannerFileNotSet_ShouldBeValid(PrintSettings printSettings, string? name)
    {
        //  Arrange
        printSettings.UseBannerFile = false;

        //  Act
        ValidateOptionsResult result = _sut.Validate(name, printSettings);

        //  Assert
        result.Succeeded.Should().BeTrue();
    }

    [Theory]
    [VeloAutoData]
    public void UseBannerFileSet_FileExists_ShouldBeValid(PrintSettings printSettings, string? name)
    {
        //  Arrange
        printSettings.UseBannerFile = true;
        printSettings.BannerFilePath = "BraunauMobil.VeloBasar.dll";

        //  Act
        ValidateOptionsResult result = _sut.Validate(name, printSettings);

        //  Assert
        result.Succeeded.Should().BeTrue();
    }

    [Theory]
    [VeloAutoData]
    public void UseBannerFileSet_FileExists_ShouldBeInValid(PrintSettings printSettings, string? name)
    {
        //  Arrange
        printSettings.UseBannerFile = true;

        //  Act
        ValidateOptionsResult result = _sut.Validate(name, printSettings);

        //  Assert
        result.Failed.Should().BeTrue();
    }
}
