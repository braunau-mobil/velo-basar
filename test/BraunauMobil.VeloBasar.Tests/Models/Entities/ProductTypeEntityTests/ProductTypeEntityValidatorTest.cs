using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductTypeEntityTests;

public class ProductTypeEntityValidatorTest
{
    private readonly ProductTypeEntityValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void NoName_ShouldBeInvalid(ProductTypeEntity productType)
    {
        //  Arrange
        productType.Name = "";

        //  Act
        ValidationResult result = _sut.Validate(productType);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(productType.Name));
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(ProductTypeEntity productType)
    {
        //  Arrange

        //  Act
        ValidationResult result = _sut.Validate(productType);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
