using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.TransactionSuccessModelTests;

public class TransactionSuccessModelValidatorTests
{
    private readonly TransactionSuccessModelValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void InvalidChange_ShouldBeNull(TransactionSuccessModel model)
    {
        //  Arrange
        model.Entity.Change = new ();

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(TransactionSuccessModel.AmountGiven));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(TransactionSuccessModel model, decimal amount)
    {
        //  Arrange
        model.Entity.Change = new (amount);

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
