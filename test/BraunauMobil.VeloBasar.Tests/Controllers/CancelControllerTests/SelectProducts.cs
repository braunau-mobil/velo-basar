using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CancelControllerTests;

public class SelectProducts
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task Id_ReturnsView(int id, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        TransactionService.Setup(_ => _.GetProductsToCancelAsync(id))
            .ReturnsAsync(products);

        //  Act
        IActionResult result = await Sut.SelectProducts(id);

        //  Assert
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        SelectProductsModel model = viewResult.Model.Should().BeOfType<SelectProductsModel>().Subject;
        model.TransactionId.Should().Be(id);
        model.Products.Should().NotBeNull();
        model.Products.Should().HaveCount(products.Count);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        TransactionService.Verify(_ => _.GetProductsToCancelAsync(id), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task NoProductSelected_ReturnsViewWithErrors(int id, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        SelectProductsModel inputModel = new ()
        {
            TransactionId = id
        };
        inputModel.SetProducts(products);
        TransactionService.Setup(_ => _.GetProductsToCancelAsync(id))
            .ReturnsAsync(products);

        //  Act
        IActionResult result = await Sut.SelectProducts(inputModel);

        //  Assert
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        SelectProductsModel resultModel = viewResult.Model.Should().BeOfType<SelectProductsModel>().Subject;
        resultModel.TransactionId.Should().Be(id);
        resultModel.Products.Should().NotBeNull();
        resultModel.Products.Should().HaveCount(products.Count);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(1);

        TransactionService.Verify(_ => _.GetProductsToCancelAsync(id), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task OneProductSelected_CallsCancelAndReturnsRedirectToTransactionSuccess(int activeBasarId, int saleId, int revertId, IReadOnlyList<ProductEntity> products, string url)
    {
        //  Arrange
        SelectProductsModel inputModel = new()
        {
            ActiveBasarId = activeBasarId,
            TransactionId = saleId
        };
        inputModel.SetProducts(products);
        inputModel.Products[0].IsSelected = true;
        TransactionService.Setup(_ => _.CancelAsync(activeBasarId, saleId, inputModel.SelectedProductIds()))
            .ReturnsAsync(revertId);
        TransactionRouter.Setup(_ => _.ToSucess(revertId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.SelectProducts(inputModel);

        //  Assert
        result.Should().NotBeNull();
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        TransactionService.Verify(_ => _.CancelAsync(activeBasarId, saleId, inputModel.SelectedProductIds()), Times.Once());
        TransactionRouter.Verify(_ => _.ToSucess(revertId), Times.Once());
        VerifyNoOtherCalls();
    }
}
