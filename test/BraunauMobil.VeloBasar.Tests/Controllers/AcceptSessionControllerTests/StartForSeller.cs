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
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).Returns(url);
        A.CallTo(() => AcceptSessionService.CreateAsync(activeBasarId, sellerId)).Returns(entity);
        A.CallTo(() => Cookie.SetActiveAcceptSession(entity)).DoesNothing();

        //  Act
        IActionResult result = await Sut.StartForSeller(sellerId, activeBasarId);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }
        
        A.CallTo(() => Cookie.SetActiveAcceptSession(entity)).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptSessionService.CreateAsync(activeBasarId, sellerId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task WitActiveSessionIdSet_RedirectsToAcceptProductCreate(int sellerId, int activeBasarId, int activeSessionId, string url)
    {
        //  Arrange
        Sut.ViewData.SetActiveSessionId(activeSessionId);
        A.CallTo(() => AcceptProductRouter.ToCreate(activeSessionId)).Returns(url);
        A.CallTo(() => AcceptSessionService.IsSessionRunning(activeSessionId)).Returns(true);

        //  Act
        IActionResult result = await Sut.StartForSeller(sellerId, activeBasarId);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => AcceptProductRouter.ToCreate(activeSessionId)).MustHaveHappenedOnceExactly();
    }
}