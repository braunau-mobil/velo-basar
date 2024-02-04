using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.BasarEntityTests;

public class BasarEntityValidatorTest
{
    private readonly BasarEntityValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void NoName_ShouldBeInvalid(BasarEntity basar)
    {
        //  Arrange
        basar.Name = string.Empty;
        basar.ProductCommissionPercentage = 10;

        //  Act
        ValidationResult result = _sut.Validate(basar);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(basar.Name));
        }
    }

    [Theory]
    [VeloInlineAutoData(-1)]
    [VeloInlineAutoData(101)]
    [VeloInlineAutoData(999)]
    public void ProductCommissionPercentageNotInRange_ShouldBeInvalid(int percentage, BasarEntity basar)
    {
        //  Arrange
        basar.ProductCommissionPercentage = percentage;

        //  Act
        ValidationResult result = _sut.Validate(basar);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(basar.ProductCommissionPercentage));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(BasarEntity basar)
    {
        //  Arrange
        basar.ProductCommissionPercentage = 10;

        //  Act
        ValidationResult result = _sut.Validate(basar);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
