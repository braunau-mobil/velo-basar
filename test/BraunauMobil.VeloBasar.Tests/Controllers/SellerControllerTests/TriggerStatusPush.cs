using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class TriggerStatusPush
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CallsTriggerStatusPushAndRedirectsToReferer(int activeBasarId, int sellerId, string url)
    {
        //  Arrange
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        Sut.Request.Headers.Referer = url;        

        //  Act
        IActionResult result = await Sut.TriggerStatusPush(activeBasarId, sellerId);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        SellerService.Verify(_ => _.TriggerStatusPushAsync(activeBasarId, sellerId), Times.Once);

        VerifyNoOtherCalls();
    }
}
