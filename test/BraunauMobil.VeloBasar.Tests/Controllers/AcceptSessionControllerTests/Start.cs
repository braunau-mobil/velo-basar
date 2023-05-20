using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class Start
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WitNoActiveSessionCookieSet_CreatesNewSession_And_RedirectsToSellerCreateForAcceptance(int activeBasarId, string url)
    {
        //  Arrange
        SellerRouter.Setup(_ => _.ToCreateForAcceptance())
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Start(activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);
        
        Cookie.Verify(_ => _.GetActiveAcceptSessionId(), Times.Once());
        SellerRouter.Verify(_ => _.ToCreateForAcceptance(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WitCompletedActiveSessionCookieSet_ClearesCookie_CreatesNewSession_And_RedirectsToSellerCreateForAcceptance(int activeBasarId, int activeSessionId, string url)
    {
        //  Arrange
        Cookie.Setup(_ => _.GetActiveAcceptSessionId())
            .Returns(activeSessionId);
        SellerRouter.Setup(_ => _.ToCreateForAcceptance())
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Start(activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        Cookie.Verify(_ => _.GetActiveAcceptSessionId(), Times.Once());
        Cookie.Verify(_ => _.ClearActiveAcceptSession(), Times.Once());
        AcceptSessionService.Verify(_ => _.IsSessionRunning(activeSessionId), Times.Once());
        SellerRouter.Verify(_ => _.ToCreateForAcceptance(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WitRunningActiveSessionCookieSet_RedirectsToAcceptProductCreate(int activeBasarId, int activeSessionId, string url)
    {
        //  Arrange
        Cookie.Setup(_ => _.GetActiveAcceptSessionId())
            .Returns(activeSessionId);
        AcceptProductRouter.Setup(_ => _.ToCreate(activeSessionId))
            .Returns(url);
        AcceptSessionService.Setup(_ => _.IsSessionRunning(activeSessionId))
            .ReturnsAsync(true);

        //  Act
        IActionResult result = await Sut.Start(activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        Cookie.Verify(_ => _.GetActiveAcceptSessionId(), Times.Once());
        AcceptSessionService.Verify(_ => _.IsSessionRunning(activeSessionId), Times.Once());
        AcceptProductRouter.Verify(_ => _.ToCreate(activeSessionId), Times.Once());
        VerifyNoOtherCalls();
    }
}