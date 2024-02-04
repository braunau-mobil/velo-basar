using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.SellerSearchModelTests;

public class SellerSearchModelValidatorTests
{
    private readonly SellerSearchModelValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void FirstNameIsNull_ShouldBeInvalid(SellerSearchModel model)
    {
        //  Arrange
        model.FirstName = null!;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.FirstName));
        }
    }

    [Theory]
    [VeloAutoData]
    public void LastNameIsNull_ShouldBeInvalid(SellerSearchModel model)
    {
        //  Arrange
        model.LastName = null!;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.LastName));
        }
    }

    [Theory]
    [VeloAutoData]
    public void FirstNameAndLastNameIsNull_ShouldBeInvalid(SellerSearchModel model)
    {
        //  Arrange
        model.FirstName = null!;
        model.LastName = null!;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.FirstName));
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.LastName));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(SellerSearchModel model)
    {
        //  Arrange

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
