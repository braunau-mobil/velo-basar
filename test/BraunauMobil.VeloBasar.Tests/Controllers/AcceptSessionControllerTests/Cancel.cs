using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class Cancel
    : TestBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task WithId_CompletedSession_CallsGet_And_ReturnsBadRequest(bool returnToList)
    {
        int sessionId = Fixture.Create<int>();

        //  Arrange
        A.CallTo(() => AcceptSessionService.GetAsync(sessionId)).Returns(new AcceptSessionEntity { EndTimeStamp = Fixture.Create<DateTime>() });

        //  Act
        IActionResult result = await Sut.Cancel(sessionId, returnToList);

        //  Assert
        using (new AssertionScope())
        {
            BadRequestObjectResult badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequest.Value.Should().BeOfType<LocalizedString>();
            badRequest.StatusCode.Should().Be(400);
        }

        A.CallTo(() => AcceptSessionService.GetAsync(sessionId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task WithIdAndReturnToList_CallsDeleteClearsCookie_AndReturnsRedirectToList(int sessionId, string url)
    {
        //  Arrange
        A.CallTo(() => AcceptSessionService.GetAsync(sessionId)).Returns(new AcceptSessionEntity { Id = sessionId, EndTimeStamp = null });
        A.CallTo(() => AcceptSessionRouter.ToList()).Returns(url);
        A.CallTo(() => AcceptSessionService.DeleteAsync(sessionId)).DoesNothing();
        A.CallTo(() => Cookie.ClearActiveAcceptSession()).DoesNothing();

        //  Act
        IActionResult result = await Sut.Cancel(sessionId, true);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => Cookie.ClearActiveAcceptSession()).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptSessionRouter.ToList()).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptSessionService.GetAsync(sessionId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptSessionService.DeleteAsync(sessionId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task WithId_CallsDeleteClearsCookie_AndReturnsRedirectToSellerDetails(int sessionId, int sellerId, string url)
    {
        //  Arrange
        A.CallTo(() => AcceptSessionService.GetAsync(sessionId)).Returns(new AcceptSessionEntity { Id = sessionId, SellerId = sellerId, EndTimeStamp = null });
        A.CallTo(() => SellerRouter.ToDetails(sellerId)).Returns(url);
        A.CallTo(() => AcceptSessionService.DeleteAsync(sessionId)).DoesNothing();
        A.CallTo(() => Cookie.ClearActiveAcceptSession()).DoesNothing();

        //  Act
        IActionResult result = await Sut.Cancel(sessionId, false);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => Cookie.ClearActiveAcceptSession()).MustHaveHappenedOnceExactly();
        A.CallTo(() => SellerRouter.ToDetails(sellerId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptSessionService.GetAsync(sessionId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptSessionService.DeleteAsync(sessionId)).MustHaveHappenedOnceExactly();
    }
}