using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class DropDatabaseConfirmed
    : TestBase
{ 
    [Fact]
    public async Task DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(false);

        //  Act
        IActionResult result = await Sut.DropDatabaseConfirmed();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;
        
        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task DevToolsEnabled_CallsDropDatabaseAndReturnsRedirectToHome(string url)
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(true);
        Router.Setup(_ => _.ToHome())
            .Returns(url);

        //  Act
        IActionResult result = await Sut.DropDatabaseConfirmed();

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        DataGeneratorService.Verify(_ => _.DropDatabaseAsync(), Times.Once());
        Router.Verify(_ => _.ToHome(), Times.Once());
        VerifyNoOtherCalls();
    }
}
