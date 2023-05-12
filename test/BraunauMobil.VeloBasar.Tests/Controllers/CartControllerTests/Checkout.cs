using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Checkout
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task EmptyCart_ReturnsNotAllowed(int activeBasarId)
    {
        //  Arrange
        Cookie.Setup(_ => _.GetCart())
            .Returns(new List<int>());

        //  Act
        IActionResult result = await Sut.Checkout(activeBasarId);

        //  Assert
        result.Should().NotBeNull();
        StatusCodeResult statusCodeResult = result.Should().BeOfType<StatusCodeResult>().Subject;
        statusCodeResult.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed);

        Cookie.Verify(_ => _.GetCart(), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task ProductsInCart_CallsCheckoutAndReturnsTransactionSuccess(int activeBasarId, IList<int> cart, int saleId, string url)
    {
        //  Arrange
        Cookie.Setup(_ => _.GetCart())
            .Returns(cart);
        TransactionService.Setup(_ => _.CheckoutAsync(activeBasarId, cart))
            .ReturnsAsync(saleId);
        TransactionRouter.Setup(_ => _.ToSucess(saleId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Checkout(activeBasarId);

        //  Assert
        result.Should().NotBeNull();
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        Cookie.Verify(_ => _.GetCart(), Times.Once());
        Cookie.Verify(_ => _.ClearCart(), Times.Once());
        TransactionService.Verify(_ => _.CheckoutAsync(activeBasarId, cart), Times.Once());
        TransactionRouter.Verify(_ => _.ToSucess(saleId), Times.Once());
        VerifyNoOtherCalls();
    }
}
