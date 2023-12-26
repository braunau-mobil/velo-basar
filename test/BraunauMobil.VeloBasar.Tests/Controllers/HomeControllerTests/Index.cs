using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.HomeControllerTests;

public class Index
    : TestBase
{ 
    [Theory]
    [AutoData]
    public void DatabaseNotInitialized_ReturnsRedirectToInitialSetup(string url)
    {
        //  Arrange
        AppContext.Setup(_ => _.IsDatabaseInitialized())
            .Returns(false);
        SetupRouter.Setup(_ => _.ToInitialSetup())
            .Returns(url);

        //  Act
        IActionResult result = Sut.Index();

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);
        
        AppContext.Verify(_ => _.IsDatabaseInitialized(), Times.Once());
        SetupRouter.Verify(_ => _.ToInitialSetup(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void DatabaseInitialized_ReturnsRedirectToActiveBasar(string url)
    {
        //  Arrange
        AppContext.Setup(_ => _.IsDatabaseInitialized())
            .Returns(true);
        BasarRouter.Setup(_ => _.ToActiveBasarDetails())
            .Returns(url);

        //  Act
        IActionResult result = Sut.Index();

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AppContext.Verify(_ => _.IsDatabaseInitialized(), Times.Once());
        BasarRouter.Verify(_ => _.ToActiveBasarDetails(), Times.Once());
        VerifyNoOtherCalls();
    }
}
