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
        SellerRouter.Setup(_ => _.ToCreateForAcceptance())
            .Returns(url);

        //  Act
        IActionResult result = Sut.Start(activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);
        
        SellerRouter.Verify(_ => _.ToCreateForAcceptance(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public void WitRunningActiveSessionIdSet_RedirectsToAcceptProductCreate(int activeBasarId, int activeSessionId, string url)
    {
        //  Arrange
        Sut.ViewData.SetActiveSessionId(activeSessionId);
        AcceptProductRouter.Setup(_ => _.ToCreate(activeSessionId))
            .Returns(url);

        //  Act
        IActionResult result = Sut.Start(activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AcceptProductRouter.Verify(_ => _.ToCreate(activeSessionId), Times.Once());
        VerifyNoOtherCalls();
    }
}