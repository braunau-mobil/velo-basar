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
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeNull();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }
}
