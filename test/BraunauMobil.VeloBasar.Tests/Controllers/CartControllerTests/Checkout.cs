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
        StatusCodeResult statusCode = result.Should().BeOfType<StatusCodeResult>().Subject;
        statusCode.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed);

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
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        Cookie.Verify(_ => _.GetCart(), Times.Once());
        Cookie.Verify(_ => _.ClearCart(), Times.Once());
        TransactionService.Verify(_ => _.CheckoutAsync(activeBasarId, cart), Times.Once());
        TransactionRouter.Verify(_ => _.ToSucess(saleId), Times.Once());
        VerifyNoOtherCalls();
    }
}
