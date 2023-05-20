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
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(details);
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        BasarService.Verify(_ => _.GetDetailsAsync(basarId), Times.Once());
        VerifyNoOtherCalls();
    }
}
