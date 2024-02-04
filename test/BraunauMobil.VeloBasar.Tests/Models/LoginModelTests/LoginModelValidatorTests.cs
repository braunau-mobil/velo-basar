using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;
using System.Net.Mail;

namespace BraunauMobil.VeloBasar.Tests.Models.InitializationConfigurationTests;

public class LoginModelValidatorTests
{
    private readonly LoginModelValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void EmailEmpty_ShouldBeInvalid(LoginModel model)
    {
        //  Arrange
        model.Email = "";

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Email));
        }
    }

    [Theory]
    [VeloAutoData]
    public void EmailNotAnValidEmailAddress_ShouldBeInvalid(LoginModel model, string adminEmail)
    {
        //  Arrange
        model.Email = adminEmail;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Email));
        }
    }

    [Theory]
    [VeloAutoData]
    public void PasswordNull_ShouldBeInvalid(LoginModel model)
    {
        //  Arrange
        model.Password = null!;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Password));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(LoginModel model, MailAddress eMailAddress)
    {
        //  Arrange
        model.Email = eMailAddress.Address;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
