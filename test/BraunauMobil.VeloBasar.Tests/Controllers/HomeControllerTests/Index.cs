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
        A.CallTo(() => AppContext.IsDatabaseInitialized()).Returns(false);
        A.CallTo(() => SetupRouter.ToInitialSetup()).Returns(url);

        //  Act
        IActionResult result = Sut.Index();

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }
        
        A.CallTo(() => AppContext.IsDatabaseInitialized()).MustHaveHappenedOnceExactly();
        A.CallTo(() => SetupRouter.ToInitialSetup()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public void DatabaseInitialized_ReturnsRedirectToActiveBasar(string url)
    {
        //  Arrange
        A.CallTo(() => AppContext.IsDatabaseInitialized()).Returns(true);
        A.CallTo(() => BasarRouter.ToActiveBasarDetails()).Returns(url);

        //  Act
        IActionResult result = Sut.Index();

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => AppContext.IsDatabaseInitialized()).MustHaveHappenedOnceExactly();
        A.CallTo(() => BasarRouter.ToActiveBasarDetails()).MustHaveHappenedOnceExactly();
    }
}
