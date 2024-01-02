using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Edit
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task WithId_GetIsCalledAndReturnsView(int productId, ProductEntity product)
    {
        //  Arrange
        ProductDetailsModel model = Fixture.Create<ProductDetailsModel>();
        A.CallTo(() => ProductService.GetAsync(productId)).Returns(product);

        //  Act
        IActionResult result = await Sut.Edit(productId);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(product);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => ProductService.GetAsync(productId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task ValidModel_CallsUpdateAndReturnsRedirectToDetails(ProductEntity product, string url)
    {
        //  Arrage
        A.CallTo(() => ProductService.UpdateAsync(product)).DoesNothing();
        A.CallTo(() => ProductRouter.ToDetails(product.Id)).Returns(url);

        //  Act
        IActionResult result = await Sut.Edit(product);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => ProductService.UpdateAsync(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductRouter.ToDetails(product.Id)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task InvalidModel_ReturnsView(ProductEntity product)
    {
        //  Arrage
        product.Price = 0;

        //  Act
        IActionResult result = await Sut.Edit(product);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(product);
            view.ViewData.ModelState.IsValid.Should().BeFalse();
        }
    }
}
