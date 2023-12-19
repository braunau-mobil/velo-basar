using BraunauMobil.VeloBasar.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class StartForSeller
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WitNoActiveSessionIdSet_CreatesNewSession_SetsAsActiveSession_And_RedirectsToAcceptProductCreate(int sellerId, int activeBasarId, int sessionId, string url)
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
        
        Cookie.Verify(_ => _.SetActiveAcceptSession(entity), Times.Once());
        AcceptProductRouter.Verify(_ => _.ToCreate(sessionId), Times.Once());
        AcceptSessionService.Verify(_ => _.CreateAsync(activeBasarId, sellerId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WitActiveSessionIdSet_RedirectsToAcceptProductCreate(int sellerId, int activeBasarId, int activeSessionId, string url)
    {
        //  Arrange
        Sut.ViewData.SetActiveSessionId(activeSessionId);
        AcceptProductRouter.Setup(_ => _.ToCreate(activeSessionId))
            .Returns(url);
        AcceptSessionService.Setup(_ => _.IsSessionRunning(activeSessionId))
            .ReturnsAsync(true);

        //  Act
        IActionResult result = await Sut.StartForSeller(sellerId, activeBasarId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AcceptProductRouter.Verify(_ => _.ToCreate(activeSessionId), Times.Once());
        VerifyNoOtherCalls();
    }
}