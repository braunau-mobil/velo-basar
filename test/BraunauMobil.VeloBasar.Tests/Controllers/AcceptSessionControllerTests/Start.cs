using BraunauMobil.VeloBasar.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class Start
    : TestBase
{
    [Theory]
    [AutoData]
    public void WitNoActiveSessionIdSet_CreatesNewSession_And_RedirectsToSellerCreateForAcceptance(int activeBasarId, string url)
    {
        //  Arrange
        A.CallTo(() => SellerRouter.ToCreateForAcceptance()).Returns(url);

        //  Act
        IActionResult result = Sut.Start(activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => SellerRouter.ToCreateForAcceptance()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public void WitRunningActiveSessionIdSet_RedirectsToAcceptProductCreate(int activeBasarId, int activeSessionId, string url)
    {
        //  Arrange
        Sut.ViewData.SetActiveSessionId(activeSessionId);
        A.CallTo(() => AcceptProductRouter.ToCreate(activeSessionId)).Returns(url);

        //  Act
        IActionResult result = Sut.Start(activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => AcceptProductRouter.ToCreate(activeSessionId)).MustHaveHappenedOnceExactly();
    }
}