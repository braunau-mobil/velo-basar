using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CancelControllerTests;

public class SelectSale
    : TestBase
{
    [Fact]
    public void NoParameter_ReturnsViewAndSelectSaleModel()
    {
        //  Arrage

        //  Act
        IActionResult result = Sut.SelectSale();

        // Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeTrue();

            view.Model.Should().NotBeNull();
            view.Model.Should().BeOfType<SelectSaleModel>();
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task WithValidModel_CallsFindAsyncAndReturnsRedirectToSelectProducts(string url)
    {
        //  Arrange
        SelectSaleModel model = Fixture.Create<SelectSaleModel>();
        TransactionEntity sale = Fixture.Create<TransactionEntity>();
        ProductToTransactionEntity productToTransaction = Fixture.BuildProductToTransactionEntity(sale).Create();
        productToTransaction.Product.ValueState = ValueState.NotSettled;
        productToTransaction.Product.StorageState = StorageState.Sold;
        sale.Products.Add(productToTransaction);

        A.CallTo(() => TransactionService.FindAsync(model.BasarId, TransactionType.Sale, model.SaleNumber)).Returns(sale);
        A.CallTo(() => CancelRouter.ToSelectProducts(sale.Id)).Returns(url);

        //  Act
        IActionResult result = await Sut.SelectSale(model);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => TransactionService.FindAsync(model.BasarId, TransactionType.Sale, model.SaleNumber)).MustHaveHappenedOnceExactly();
        A.CallTo(() => CancelRouter.ToSelectProducts(sale.Id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task NoProductToCancel_ReturnsView()
    {
        //  Arrange
        SelectSaleModel model = Fixture.Create<SelectSaleModel>();
        TransactionEntity sale = Fixture.Create<TransactionEntity>();
        sale.Products.Clear();
        ProductToTransactionEntity productToTransaction = Fixture.BuildProductToTransactionEntity(sale).Create();
        productToTransaction.Product.ValueState = ValueState.NotSettled;
        productToTransaction.Product.StorageState = StorageState.Available;
        sale.Products.Add(productToTransaction);

        A.CallTo(() => TransactionService.FindAsync(model.BasarId, TransactionType.Sale, model.SaleNumber)).Returns(sale);

        //  Act
        IActionResult result = await Sut.SelectSale(model);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();

            view.Model.Should().NotBeNull();
            view.Model.Should().Be(model);
        }

        A.CallTo(() => TransactionService.FindAsync(model.BasarId, TransactionType.Sale, model.SaleNumber)).MustHaveHappenedOnceExactly();
    }
}
