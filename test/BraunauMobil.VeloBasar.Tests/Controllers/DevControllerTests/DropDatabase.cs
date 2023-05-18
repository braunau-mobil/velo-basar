using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class DropDatabase
    : TestBase
{ 
    [Fact]
    public void DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(false);

        //  Act
        IActionResult result = Sut.DropDatabase();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;
        
        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Fact]
    public void DevToolsEnabled_ReturnsView()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(true);

        //  Act
        IActionResult result = Sut.DropDatabase();

        //  Assert
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeNull();
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }
}
