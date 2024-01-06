using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class ProductEntityValidatorTest
{
    private readonly ProductEntityValidator _sut = new(new StringLocalizerMock<SharedResources>());

    [Theory]
    [VeloAutoData]
    public void NoBrand_ShouldBeInvalid(ProductEntity product)
    {
        //  Arrange
        product.Brand = "";

        //  Act
        ValidationResult result = _sut.Validate(product);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(product.Brand));
    }

    [Theory]
    [VeloAutoData]
    public void NoProductType_ShouldBeInvalid(ProductEntity product)
    {
        //  Arrange
        product.TypeId = 0;

        //  Act
        ValidationResult result = _sut.Validate(product);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(product.TypeId));
    }

    [Theory]
    [VeloInlineAutoData(null!)]
    [VeloInlineAutoData("This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.This is a really long description.")]
    public void InvalidDescription_ShouldBeInvalid(string description, ProductEntity product)
    {
        //  Arrange
        product.Description = description;

        //  Act
        ValidationResult result = _sut.Validate(product);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(product.Description));
    }

    [Theory]
    [VeloInlineAutoData(0)]
    [VeloInlineAutoData(-100)]
    public void PriceIsNotValid_ShouldBeInvalid(decimal price, ProductEntity product)
    {
        //  Arrange
        product.Price = price;

        //  Act
        ValidationResult result = _sut.Validate(product);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(product.Price));
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(ProductEntity product, int productTypeId)
    {
        //  Arrange
        product.TypeId = productTypeId;

        //  Act
        ValidationResult result = _sut.Validate(product);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
