using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.ProductControllerTests;

public class Details
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CallsGetDetailsAndReturnsView(int activeBasarId, int productId)
    {
        //  Arrange
        ProductDetailsModel model = Fixture.BuildProductDetailsModel().Create();
        A.CallTo(() => ProductService.GetDetailsAsync(activeBasarId, productId)).Returns(model);

        //  Act
        IActionResult result = await Sut.Details(activeBasarId, productId);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => ProductService.GetDetailsAsync(activeBasarId, productId)).MustHaveHappenedOnceExactly();
    }
}
