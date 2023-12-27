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
        A.CallTo(() => SellerService.SettleAsync(activeBasarId, sellerId)).Returns(settlemendId);
        A.CallTo(() => TransactionRouter.ToSucess(settlemendId)).Returns(url);

        //  Act
        IActionResult result = await Sut.Settle(activeBasarId, sellerId);

        //  Assert
        using (new AssertionScope())
        { 
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => SellerService.SettleAsync(activeBasarId, sellerId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionRouter.ToSucess(settlemendId)).MustHaveHappenedOnceExactly();
    }
}
