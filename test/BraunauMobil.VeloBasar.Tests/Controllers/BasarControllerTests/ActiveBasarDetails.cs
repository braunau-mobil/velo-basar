using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.BasarControllerTests;

public class ActiveBasarDetails
    : TestBase
{
    [Theory]
    [AutoData]
    public void RedirectsToBasarDetails(int activeBasarId, string url)
    {
        //  Arrange
        A.CallTo(() => BasarRouter.ToDetails(activeBasarId)).Returns(url);

        //  Act
        IActionResult result = Sut.ActiveBasarDetails(activeBasarId);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => BasarRouter.ToDetails(activeBasarId)).MustHaveHappenedOnceExactly();
    }
}
