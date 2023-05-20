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
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        view.Model.Should().NotBeNull();
        view.Model.Should().BeOfType<SelectSaleModel>();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithValidModel_CallsFindAsyncAndReturnsRedirectToSelectProducts(string url)
    {
        //  Arrange
        SelectSaleModel model = Fixture.BuildSelectSaleModel().Create();
        TransactionEntity sale = Fixture.BuildTransaction().Create();
        ProductToTransactionEntity productToTransaction = Fixture.BuildProductToTransactionEntity(sale).Create();
        productToTransaction.Product.ValueState = ValueState.NotSettled;
        productToTransaction.Product.StorageState = StorageState.Sold;
        sale.Products.Add(productToTransaction);

        TransactionService.Setup(_ => _.FindAsync(model.ActiveBasarId, TransactionType.Sale, model.SaleNumber))
            .ReturnsAsync(sale);
        CancelRouter.Setup(_ => _.ToSelectProducts(sale.Id))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.SelectSale(model);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        TransactionService.Verify(_ => _.FindAsync(model.ActiveBasarId, TransactionType.Sale, model.SaleNumber), Times.Once());
        CancelRouter.Verify(_ => _.ToSelectProducts(sale.Id), Times.Once());
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task NoProductToCancel_ReturnsView()
    {
        //  Arrange
        SelectSaleModel model = Fixture.BuildSelectSaleModel().Create();
        TransactionEntity sale = Fixture.BuildTransaction().Create();
        sale.Products.Clear();
        ProductToTransactionEntity productToTransaction = Fixture.BuildProductToTransactionEntity(sale).Create();
        productToTransaction.Product.ValueState = ValueState.NotSettled;
        productToTransaction.Product.StorageState = StorageState.Available;
        sale.Products.Add(productToTransaction);

        TransactionService.Setup(_ => _.FindAsync(model.ActiveBasarId, TransactionType.Sale, model.SaleNumber))
            .ReturnsAsync(sale);

        //  Act
        IActionResult result = await Sut.SelectSale(model);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeFalse();

        view.Model.Should().NotBeNull();
        view.Model.Should().Be(model);

        TransactionService.Verify(_ => _.FindAsync(model.ActiveBasarId, TransactionType.Sale, model.SaleNumber), Times.Once());
        VerifyNoOtherCalls();
    }
}
