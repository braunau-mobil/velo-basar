using BraunauMobil.VeloBasar.Cookies;
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
        AcceptSessionService.Setup(_ => _.GetAsync(sessionId))
            .ReturnsAsync(new AcceptSessionEntity { EndTimeStamp = Fixture.Create<DateTime>() });

        //  Act
        IActionResult result = await Sut.Cancel(sessionId, returnToList);

        //  Assert
        result.Should().NotBeNull();
        BadRequestObjectResult badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.Value.Should().BeOfType<LocalizedString>();
        badRequestResult.StatusCode.Should().Be(400);

        AcceptSessionService.Verify(_ => _.GetAsync(sessionId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithIdAndReturnToList_CallsDeleteClearsCookie_AndReturnsRedirectToList(int sessionId, string url)
    {
        //  Arrange
        AcceptSessionService.Setup(_ => _.GetAsync(sessionId))
            .ReturnsAsync(new AcceptSessionEntity { Id = sessionId, EndTimeStamp = null });
        AcceptSessionRouter.Setup(_ => _.ToList())
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Cancel(sessionId, true);

        //  Assert
        result.Should().NotBeNull();
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        Cookie.Verify(_ => _.ClearActiveAcceptSession(), Times.Once());
        AcceptSessionRouter.Verify(_ => _.ToList(), Times.Once());
        AcceptSessionService.Verify(_ => _.GetAsync(sessionId), Times.Once());
        AcceptSessionService.Verify(_ => _.DeleteAsync(sessionId), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithId_CallsDeleteClearsCookie_AndReturnsRedirectToSellerDetails(int sessionId, int sellerId, string url)
    {
        //  Arrange
        AcceptSessionService.Setup(_ => _.GetAsync(sessionId))
            .ReturnsAsync(new AcceptSessionEntity { Id = sessionId, SellerId = sellerId, EndTimeStamp = null });
        SellerRouter.Setup(_ => _.ToDetails(sellerId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Cancel(sessionId, false);

        //  Assert
        result.Should().NotBeNull();
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        Cookie.Verify(_ => _.ClearActiveAcceptSession(), Times.Once());
        SellerRouter.Verify(_ => _.ToDetails(sellerId), Times.Once());
        AcceptSessionService.Verify(_ => _.GetAsync(sessionId), Times.Once());
        AcceptSessionService.Verify(_ => _.DeleteAsync(sessionId), Times.Once());
        VerifyNoOtherCalls();
    }
}