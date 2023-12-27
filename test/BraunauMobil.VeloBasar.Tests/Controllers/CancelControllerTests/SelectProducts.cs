using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CancelControllerTests;

public class SelectProducts
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task Id_ReturnsView(int id, IReadOnlyList<ProductEntity> products)
    {
        //  Arrange
        A.CallTo(() => TransactionService.GetProductsToCancelAsync(id)).Returns(products);

        //  Act
        IActionResult result = await Sut.SelectProducts(id);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            SelectProductsModel model = view.Model.Should().BeOfType<SelectProductsModel>().Subject;
            model.TransactionId.Should().Be(id);
            model.Products.Should().NotBeNull();
            model.Products.Should().HaveCount(products.Count);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => TransactionService.GetProductsToCancelAsync(id)).MustHaveHappenedOnceExactly();
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
        A.CallTo(() => TransactionService.GetProductsToCancelAsync(id)).Returns(products);

        //  Act
        IActionResult result = await Sut.SelectProducts(inputModel);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            SelectProductsModel resultModel = view.Model.Should().BeOfType<SelectProductsModel>().Subject;
            resultModel.TransactionId.Should().Be(id);
            resultModel.Products.Should().NotBeNull();
            resultModel.Products.Should().HaveCount(products.Count);
        }

        A.CallTo(() => TransactionService.GetProductsToCancelAsync(id)).MustHaveHappenedOnceExactly();
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
        A.CallTo(() => TransactionService.CancelAsync(activeBasarId, saleId, inputModel.SelectedProductIds())).Returns(revertId);
        A.CallTo(() => TransactionRouter.ToSucess(revertId)).Returns(url);

        //  Act
        IActionResult result = await Sut.SelectProducts(inputModel);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => TransactionService.CancelAsync(activeBasarId, saleId, inputModel.SelectedProductIds())).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionRouter.ToSucess(revertId)).MustHaveHappenedOnceExactly();
    }
}
