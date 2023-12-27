using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class DropDatabase
    : TestBase
{ 
    [Fact]
    public void DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(false);

        //  Act
        IActionResult result = Sut.DropDatabase();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void DevToolsEnabled_ReturnsView()
    {
        //  Arrange
        A.CallTo(() => AppContext.DevToolsEnabled()).Returns(true);

        //  Act
        IActionResult result = Sut.DropDatabase();

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => AppContext.DevToolsEnabled()).MustHaveHappenedOnceExactly();
    }
}
