using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.SelectSaleModelTests;

public class SelectSaleModelValidatorTests
{
    private readonly SelectSaleModelValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void SaleIsNull_ShouldBeInvalid(SelectSaleModel model)
    {
        //  Arrange
        model.Sale = null;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.SaleNumber));
        }
    }

    [Theory]
    [VeloAutoData]
    public void SaleHasNoProducts_ShouldBeInvalid(SelectSaleModel model, TransactionEntity sale)
    {
        //  Arrange
        sale.Type = TransactionType.Sale;
        model.Sale = sale;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.SaleNumber));
        }
    }

    [Theory]
    [VeloAutoData]
    public void SaleHasNoProductsToCancel_ShouldBeInvalid(SelectSaleModel model, TransactionEntity sale, ProductEntity[] products)
    {
        //  Arrange
        sale.Type = TransactionType.Sale;
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(StorageState.Sold);
        fixture.ExcludeEnumValues(ValueState.NotSettled);
        foreach (ProductEntity product in products)
        {
            product.StorageState = fixture.Create<StorageState>();
            product.ValueState = fixture.Create<ValueState>();
            
            sale.Products.Add(new ProductToTransactionEntity(sale, product));
        }
        model.Sale = sale;

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.SaleNumber));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(SelectSaleModel model, TransactionEntity sale, ProductEntity[] productsToCancel, ProductEntity[] otherProducts)
    {
        //  Arrange
        sale.Type = TransactionType.Sale;
        foreach (ProductEntity product in productsToCancel)
        {
            product.StorageState = StorageState.Sold;
            product.ValueState = ValueState.NotSettled;

            sale.Products.Add(new ProductToTransactionEntity(sale, product));
        }
        foreach (ProductEntity product in otherProducts)
        {
            sale.Products.Add(new ProductToTransactionEntity(sale, product));
        }
        model.Sale = sale;

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
