using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class DangerZone
    : TestBase
{ 
    [Fact]
    public void NoParameters_DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(false);

        //  Act
        IActionResult result = Sut.DangerZone();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;
        
        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Fact]
    public void NoParameters_DevToolsEnabled_ReturnsView()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(true);

        //  Act
        IActionResult result = Sut.DangerZone();

        //  Assert
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        DataGeneratorConfiguration model = viewResult.Model.Should().BeOfType<DataGeneratorConfiguration>().Subject;
        model.GenerateBrands.Should().Be(true);
        model.GenerateCountries.Should().Be(true);
        model.GenerateProductTypes.Should().Be(true);
        model.GenerateZipCodes.Should().Be(true);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithConfig_DevToolsNotEnabled_ReturnsUnauthorized(DataGeneratorConfiguration configuration)
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(false);

        //  Act
        IActionResult result = await Sut.DangerZone(configuration);

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithConfig_DevToolsEnabled_ContextualizesServiceAndCallsGenerateAndClearsCookiesAndReturnsRedirectToHome(DataGeneratorConfiguration configuration, string url)
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(true);
        Router.Setup(_ => _.ToHome())
            .Returns(url);
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        //  Act
        IActionResult result = await Sut.DangerZone(configuration);

        //  Assert
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        DataGeneratorService.Verify(_ => _.Contextualize(configuration), Times.Once());
        DataGeneratorService.Verify(_ => _.GenerateAsync(), Times.Once());
        Router.Verify(_ => _.ToHome(), Times.Once());
        VerifyNoOtherCalls();
    }
}
