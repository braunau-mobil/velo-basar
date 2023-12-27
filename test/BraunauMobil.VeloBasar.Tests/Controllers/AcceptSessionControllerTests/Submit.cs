using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class Submit
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CallsSubmit_And_RedirectsToSuccess(int sessionId, int acceptanceId, string url)
    {
        //  Arrange
        A.CallTo(() => AcceptSessionService.SubmitAsync(sessionId)).Returns(acceptanceId);
        A.CallTo(() => TransactionRouter.ToSucess(acceptanceId)).Returns(url);

        //  Act
        IActionResult result = await Sut.Submit(sessionId);

        //  Act & Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => AcceptSessionService.SubmitAsync(sessionId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionRouter.ToSucess(acceptanceId)).MustHaveHappenedOnceExactly();
    }
}