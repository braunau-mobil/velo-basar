using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Details
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task CallsGetDetailsAndReturnsView(int productId)
    {
        //  Arrange
        ProductDetailsModel model = Fixture.Create<ProductDetailsModel>();
        A.CallTo(() => ProductService.GetDetailsAsync(productId)).Returns(model);

        //  Act
        IActionResult result = await Sut.Details(productId);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => ProductService.GetDetailsAsync(productId)).MustHaveHappenedOnceExactly();
    }
}
