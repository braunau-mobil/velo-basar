using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.HomeControllerTests;

public class Index
    : TestBase
{ 
    [Theory]
    [VeloAutoData]
    public void ShouldReturnRedirectToActiveBasar(string url)
    {
        //  Arrange
        A.CallTo(() => BasarRouter.ToActiveBasarDetails()).Returns(url);

        //  Act
        IActionResult result = Sut.Index();

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => BasarRouter.ToActiveBasarDetails()).MustHaveHappenedOnceExactly();
    }
}
