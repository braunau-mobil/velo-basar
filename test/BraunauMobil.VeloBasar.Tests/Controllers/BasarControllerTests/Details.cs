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
        A.CallTo(() => BasarService.GetDetailsAsync(basarId)).Returns(details);

        //  Act
        IActionResult result = await Sut.Details(basarId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(details);
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        A.CallTo(() => BasarService.GetDetailsAsync(basarId)).MustHaveHappenedOnceExactly();
    }
}
