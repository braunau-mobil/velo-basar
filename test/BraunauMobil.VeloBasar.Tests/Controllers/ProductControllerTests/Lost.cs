using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Lost
    : TestBase
{
    [Theory]
    [AutoData]
    public void WithId_ReturnsView(int productId)
    {
        //  Arrange

        //  Act
        IActionResult result = Sut.Lost(productId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        ProductAnnotateModel annotateModel = view.Model.Should().BeOfType<ProductAnnotateModel>().Subject;
        annotateModel.ProductId.Should().Be(productId);
        view.ViewData.ModelState.IsValid.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public async Task ValidModel_CallsSetLostAndReturnsRedirectToDetails(ProductAnnotateModel annotateModel, string url)
    {
        //  Arrage
        A.CallTo(() => ProductRouter.ToDetails(annotateModel.ProductId)).Returns(url);
        A.CallTo(() => ProductService.SetLostAsync(annotateModel.ProductId, annotateModel.Notes)).DoesNothing();

        //  Act
        IActionResult result = await Sut.Lost(annotateModel);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => ProductService.SetLostAsync(annotateModel.ProductId, annotateModel.Notes)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductRouter.ToDetails(annotateModel.ProductId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task InvalidModel_ReturnsView(ProductAnnotateModel annotateModel)
    {
        //  Arrage
        annotateModel.Notes = "";

        //  Act
        IActionResult result = await Sut.Lost(annotateModel);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(annotateModel);
        view.ViewData.ModelState.IsValid.Should().BeFalse();
    }
}
