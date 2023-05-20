using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class Settle
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task CallsSettleAndReturnsRedirectoToTransactionSucess(int activeBasarId, int sellerId, int settlemendId, string url)
    {
        //  Arrange
        SellerService.Setup(_ => _.SettleAsync(activeBasarId, sellerId))
            .ReturnsAsync(settlemendId);
        TransactionRouter.Setup(_ => _.ToSucess(settlemendId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Settle(activeBasarId, sellerId);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        SellerService.Verify(_ => _.SettleAsync(activeBasarId, sellerId), Times.Once());
        TransactionRouter.Verify(_ => _.ToSucess(settlemendId), Times.Once());
        VerifyNoOtherCalls();
    }
}
