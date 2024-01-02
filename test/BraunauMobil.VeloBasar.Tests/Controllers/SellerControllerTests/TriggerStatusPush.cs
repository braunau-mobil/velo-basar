using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class TriggerStatusPush
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task CallsTriggerStatusPushAndRedirectsToReferer(int activeBasarId, int sellerId, string url)
    {
        //  Arrange
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        Sut.Request.Headers.Referer = url;
        A.CallTo(() => SellerService.TriggerStatusPushAsync(activeBasarId, sellerId)).DoesNothing();

        //  Act
        IActionResult result = await Sut.TriggerStatusPush(activeBasarId, sellerId);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => SellerService.TriggerStatusPushAsync(activeBasarId, sellerId)).MustHaveHappenedOnceExactly();
    }
}
