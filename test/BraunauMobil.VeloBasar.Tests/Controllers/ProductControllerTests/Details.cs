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
        ProductService.Setup(_ => _.GetDetailsAsync(activeBasarId, productId))
            .ReturnsAsync(model);

        //  Act
        IActionResult result = await Sut.Details(activeBasarId, productId);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(model);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        ProductService.Verify(_ => _.GetDetailsAsync(activeBasarId, productId), Times.Once());
        VerifyNoOtherCalls();
    }
}
