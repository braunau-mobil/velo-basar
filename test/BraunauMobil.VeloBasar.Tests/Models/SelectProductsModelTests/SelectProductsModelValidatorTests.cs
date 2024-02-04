using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Models.SelectProductsModelTests;

public class SelectProductsModelValidatorTests
{
    private readonly SelectProductsModelValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void NothingSelected_ShouldBeInvalid(SelectProductsModel model)
    {
        //  Arrange
        foreach (SelectModel<ProductEntity> selectModel in model.Products)
        {
            selectModel.IsSelected = false;
        }

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Products));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(SelectProductsModel model, List<SelectModel<ProductEntity>> selected, List<SelectModel<ProductEntity>> unselected)
    {
        //  Arrange
        foreach (SelectModel<ProductEntity> selectModel in selected)
        {
            selectModel.IsSelected = true;
        }
        foreach (SelectModel<ProductEntity> selectModel in unselected)
        {
            selectModel.IsSelected = false;
        }
        model.Products.AddRange(selected);
        model.Products.AddRange(unselected);

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
