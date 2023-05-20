using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests;

public class Delete
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CallsDelete_And_ReturnsRedirectoToCreate(int sessionId, int productId, string url)
    {
        //  Arrange
        AcceptProductRouter.Setup(_ => _.ToCreate(sessionId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Delete(sessionId, productId);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AcceptProductService.Verify(_ => _.DeleteAsync(productId), Times.Once);
        AcceptProductRouter.Verify(_ => _.ToCreate(sessionId), Times.Once);
        VerifyNoOtherCalls();
    }
}
