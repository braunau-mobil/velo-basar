using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.ExportModelTests;

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
}
