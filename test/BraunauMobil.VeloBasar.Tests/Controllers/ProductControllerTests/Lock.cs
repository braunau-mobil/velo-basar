using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Lock
    : TestBase
{
    [Theory]
    [AutoData]
    public void WithId_ReturnsView(int productId)
    {
        //  Arrange

        //  Act
        IActionResult result = Sut.Lock(productId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        ProductAnnotateModel annotateModel = view.Model.Should().BeOfType<ProductAnnotateModel>().Subject;
        annotateModel.ProductId.Should().Be(productId);
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ValidModel_CallsLockAndReturnsRedirectToDetails(ProductAnnotateModel annotateModel, string url)
    {
        //  Arrage
        ProductRouter.Setup(_ => _.ToDetails(annotateModel.ProductId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Lock(annotateModel);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        ProductService.Verify(_ => _.LockAsync(annotateModel.ProductId, annotateModel.Notes), Times.Once());
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
        IActionResult result = await Sut.Lock(annotateModel);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(annotateModel);
        view.ViewData.ModelState.IsValid.Should().BeFalse();

        VerifyNoOtherCalls();
    }
}
