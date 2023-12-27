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
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).Returns(url);
        A.CallTo(() => AcceptProductService.DeleteAsync(productId)).DoesNothing();

        //  Act
        IActionResult result = await Sut.Delete(sessionId, productId);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => AcceptProductService.DeleteAsync(productId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).MustHaveHappenedOnceExactly();
    }
}
