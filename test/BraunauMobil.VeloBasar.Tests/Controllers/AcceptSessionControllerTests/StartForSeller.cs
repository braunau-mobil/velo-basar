using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class StartForSeller
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WitNoActiveSessionCookieSet_CreatesNewSession_SetsAsActiveSession_And_RedirectsToAcceptProductCreate(int sellerId, int activeBasarId, int sessionId, string url)
    {
        //  Arrange
        AcceptSessionEntity entity = Fixture.Create<AcceptSessionEntity>();
        entity.Id = sessionId;
        AcceptProductRouter.Setup(_ => _.ToCreate(sessionId))
            .Returns(url);
        AcceptSessionService.Setup(_ => _.CreateAsync(activeBasarId, sellerId))
            .ReturnsAsync(entity);

        //  Act
        IActionResult result = await Sut.StartForSeller(sellerId, activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);
        
        Cookie.Verify(_ => _.GetActiveAcceptSessionId(), Times.Once());
        Cookie.Verify(_ => _.SetActiveAcceptSession(entity), Times.Once());
        AcceptProductRouter.Verify(_ => _.ToCreate(sessionId), Times.Once());
        AcceptSessionService.Verify(_ => _.CreateAsync(activeBasarId, sellerId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WitCompletedActiveSessionCookieSet_ClearesCookie_CreatesNewSession_SetsAsActiveSession_And_RedirectsToAcceptProductCreate(int sellerId, int activeBasarId, int newSessionId, int completedSessionId, string url)
    {
        //  Arrange
        AcceptSessionEntity entity = Fixture.Create<AcceptSessionEntity>();
        entity.Id = newSessionId;
        AcceptProductRouter.Setup(_ => _.ToCreate(newSessionId))
            .Returns(url);
        AcceptSessionService.Setup(_ => _.CreateAsync(activeBasarId, sellerId))
            .ReturnsAsync(entity);
        Cookie.Setup(_ => _.GetActiveAcceptSessionId())
            .Returns(completedSessionId);

        //  Act
        IActionResult result = await Sut.StartForSeller(sellerId, activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        Cookie.Verify(_ => _.GetActiveAcceptSessionId(), Times.Once());
        Cookie.Verify(_ => _.ClearActiveAcceptSession(), Times.Once());
        Cookie.Verify(_ => _.SetActiveAcceptSession(entity), Times.Once());
        AcceptProductRouter.Verify(_ => _.ToCreate(newSessionId), Times.Once());
        AcceptSessionService.Verify(_ => _.IsSessionRunning(completedSessionId), Times.Once());
        AcceptSessionService.Verify(_ => _.CreateAsync(activeBasarId, sellerId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WitRunningActiveSessionCookieSet_RedirectsToAcceptProductCreate(int sellerId, int activeBasarId, int activeSessionId, string url)
    {
        //  Arrange
        Cookie.Setup(_ => _.GetActiveAcceptSessionId())
            .Returns(activeSessionId);
        AcceptProductRouter.Setup(_ => _.ToCreate(activeSessionId))
            .Returns(url);
        AcceptSessionService.Setup(_ => _.IsSessionRunning(activeSessionId))
            .ReturnsAsync(true);

        //  Act
        IActionResult result = await Sut.StartForSeller(sellerId, activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        Cookie.Verify(_ => _.GetActiveAcceptSessionId(), Times.Once());
        AcceptSessionService.Verify(_ => _.IsSessionRunning(activeSessionId), Times.Once());
        AcceptProductRouter.Verify(_ => _.ToCreate(activeSessionId), Times.Once());
        VerifyNoOtherCalls();
    }
}