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
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(product);
        view.ViewData.ModelState.IsValid.Should().BeTrue();

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
        IActionResult result = await Sut.Edit(product);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

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
        IActionResult result = await Sut.Edit(product);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(product);
        view.ViewData.ModelState.IsValid.Should().BeFalse();

        VerifyNoOtherCalls();
    }
}
