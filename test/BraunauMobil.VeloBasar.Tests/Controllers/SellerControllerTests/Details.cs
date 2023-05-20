using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class Details
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ReturnsView(int activeBasarId, int sellerId)
    {
        //  Arrange
        SellerDetailsModel model = Fixture.BuildSellerDetailsModel().Create();
        SellerService.Setup(_ => _.GetDetailsAsync(activeBasarId, sellerId))
            .ReturnsAsync(model);

        //  Act
        IActionResult result = await Sut.Details(activeBasarId, sellerId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(model);
        view.ViewData.ModelState.ErrorCount.Should().Be(0);

        SellerService.Verify(_ => _.GetDetailsAsync(activeBasarId, sellerId), Times.Once());
        VerifyNoOtherCalls();
    }
}
