using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;
using System.Net.Mail;

namespace BraunauMobil.VeloBasar.Tests.Models.InitializationConfigurationTests;

public class InitializationConfigurationValidatorTest
{
    private readonly InitializationConfigurationValidator _sut = new(new StringLocalizerMock<SharedResources>());

    [Theory]
    [VeloAutoData]
    public void AdminUserEmailEmpty_ShouldBeInvalid(InitializationConfiguration config)
    {
        //  Arrange
        config.AdminUserEMail = "";

        //  Act
        ValidationResult result = _sut.Validate(config);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(config.AdminUserEMail));
        }
    }

    [Theory]
    [VeloAutoData]
    public void GenerateZipCodesSet_GenerateCountriesNotSet_ShouldBeInvalid(InitializationConfiguration config)
    {
        //  Arrange
        config.GenerateZipCodes = true;
        config.GenerateCountries = false;

        //  Act
        ValidationResult result = _sut.Validate(config);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(config.GenerateZipCodes));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(InitializationConfiguration config, MailAddress adminMailAddress)
    {
        //  Arrange
        config.AdminUserEMail = adminMailAddress.Address;
        config.GenerateZipCodes = true;
        config.GenerateCountries = true;

        //  Act
        ValidationResult result = _sut.Validate(config);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
