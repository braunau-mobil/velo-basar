using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class DropDatabaseConfirmed
    : TestBase
{ 
    [Fact]
    public async Task DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(false);

        //  Act
        IActionResult result = await Sut.DropDatabaseConfirmed();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task DevToolsEnabled_CallsDropDatabaseAndReturnsRedirectToHome(string url)
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(true);
        A.CallTo(() => DataGeneratorService.DropDatabaseAsync()).DoesNothing();
        A.CallTo(() => DataGeneratorService.CreateDatabaseAsync()).DoesNothing();
        A.CallTo(() => Router.ToHome()).Returns(url);

        //  Act
        IActionResult result = await Sut.DropDatabaseConfirmed();

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
        A.CallTo(() => DataGeneratorService.DropDatabaseAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => DataGeneratorService.CreateDatabaseAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => Router.ToHome()).MustHaveHappenedOnceExactly();
    }
}
