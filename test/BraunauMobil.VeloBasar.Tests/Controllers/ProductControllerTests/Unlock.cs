using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class UnLock
    : TestBase
{
    [Theory]
    [AutoData]
    public void WithId_ReturnsView(int productId)
    {
        //  Arrange

        //  Act
        IActionResult result = Sut.UnLock(productId);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        ProductAnnotateModel annotateModel = viewResult.Model.Should().BeOfType<ProductAnnotateModel>().Subject;
        annotateModel.ProductId.Should().Be(productId);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ValidModel_CallsUnLockAndReturnsRedirectToDetails(ProductAnnotateModel annotateModel, string url)
    {
        //  Arrage
        ProductRouter.Setup(_ => _.ToDetails(annotateModel.ProductId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.UnLock(annotateModel);

        //  Assert
        result.Should().NotBeNull();
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        ProductService.Verify(_ => _.UnlockAsync(annotateModel.ProductId, annotateModel.Notes), Times.Once());
        ProductRouter.Verify(_ => _.ToDetails(annotateModel.ProductId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task InvalidModel_ReturnsView(ProductAnnotateModel annotateModel)
    {
        //  Arrage
        annotateModel.Notes = "";

        //  Act
        IActionResult result = await Sut.UnLock(annotateModel);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(annotateModel);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(1);

        VerifyNoOtherCalls();
    }
}
