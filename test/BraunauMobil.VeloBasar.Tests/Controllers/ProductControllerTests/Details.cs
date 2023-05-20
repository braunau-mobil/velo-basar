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
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(model);
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        ProductService.Verify(_ => _.GetDetailsAsync(activeBasarId, productId), Times.Once());
        VerifyNoOtherCalls();
    }
}
