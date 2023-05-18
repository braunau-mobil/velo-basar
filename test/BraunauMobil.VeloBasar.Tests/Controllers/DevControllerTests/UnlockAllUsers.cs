using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.DevControllerTests;

public class UnlockAllUsers
    : TestBase
{ 
    [Fact]
    public async Task DevToolsNotEnabled_ReturnsUnauthorized()
    {
        //  Arrange
        AppContext.Setup(_ => _.DevToolsEnabled())
            .Returns(false);

        //  Act
        IActionResult result = await Sut.UnlockAllUsers();

        //  Assert
        UnauthorizedResult unauthorized = result.Should().BeOfType<UnauthorizedResult>().Subject;
        
        AppContext.Verify(_ => _.DevToolsEnabled(), Times.Once());
        VerifyNoOtherCalls();
    }
}
