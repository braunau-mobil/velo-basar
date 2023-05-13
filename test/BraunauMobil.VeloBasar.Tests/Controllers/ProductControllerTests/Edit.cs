using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Edit
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WithId_GetIsCalledAndReturnsView(int productId, ProductEntity product)
    {
        //  Arrange
        ProductDetailsModel model = Fixture.BuildProductDetailsModel().Create();
        ProductService.Setup(_ => _.GetAsync(productId))
            .ReturnsAsync(product);

        //  Act
        IActionResult result = await Sut.Edit(productId);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(product);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        ProductService.Verify(_ => _.GetAsync(productId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ValidModel_CallsUpdateAndReturnsRedirectToDetails(ProductEntity product, string url)
    {
        //  Arrage
        ProductRouter.Setup(_ => _.ToDetails(product.Id))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.EditAsync(product);

        //  Assert
        result.Should().NotBeNull();
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        ProductService.Verify(_ => _.UpdateAsync(product), Times.Once());
        ProductRouter.Verify(_ => _.ToDetails(product.Id), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task InvalidModel_ReturnsView(ProductEntity product)
    {
        //  Arrage
        product.Price = 0;

        //  Act
        IActionResult result = await Sut.EditAsync(product);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(product);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(1);

        VerifyNoOtherCalls();
    }
}
