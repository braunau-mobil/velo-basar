using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.BasarControllerTests;

public class Details 
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CallsGetDetailsAsync_AndRedirectsToBasarDetails(int basarId)
    {
        //  Arrange
        BasarDetailsModel details = Fixture.Create<BasarDetailsModel>();
        BasarService.Setup(_ => _.GetDetailsAsync(basarId))
            .ReturnsAsync(details);

        //  Act
        IActionResult result = await Sut.Details(basarId);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(details);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        BasarService.Verify(_ => _.GetDetailsAsync(basarId), Times.Once());
        VerifyNoOtherCalls();
    }
}
