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
        AcceptSessionService.Setup(_ => _.SubmitAsync(sessionId))
            .ReturnsAsync(acceptanceId);
        TransactionRouter.Setup(_ => _.ToSucess(acceptanceId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Submit(sessionId);

        //  Act & Assert
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);
        
        AcceptSessionService.Verify(_ => _.SubmitAsync(sessionId), Times.Once());
        TransactionRouter.Verify(_ => _.ToSucess(acceptanceId), Times.Once());
        VerifyNoOtherCalls();
    }
}