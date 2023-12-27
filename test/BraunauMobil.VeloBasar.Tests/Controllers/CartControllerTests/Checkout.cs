using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Checkout
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task EmptyCart_ReturnsNotAllowed(int activeBasarId)
    {
        //  Arrange
        A.CallTo(() => Cookie.GetCart()).Returns(new List<int>());

        //  Act
        IActionResult result = await Sut.Checkout(activeBasarId);

        //  Assert
        using (new AssertionScope())
        {
            StatusCodeResult statusCode = result.Should().BeOfType<StatusCodeResult>().Subject;
            statusCode.StatusCode.Should().Be(StatusCodes.Status405MethodNotAllowed);
        }

        A.CallTo(() => Cookie.GetCart()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [AutoData]
    public async Task ProductsInCart_CallsCheckoutAndReturnsTransactionSuccess(int activeBasarId, IList<int> cart, int saleId, string url)
    {
        //  Arrange
        A.CallTo(() => Cookie.GetCart()).Returns(cart);
        A.CallTo(() => TransactionService.CheckoutAsync(activeBasarId, cart)).Returns(saleId);
        A.CallTo(() => TransactionRouter.ToSucess(saleId)).Returns(url);
        A.CallTo(() => Cookie.ClearCart()).DoesNothing();

        //  Act
        IActionResult result = await Sut.Checkout(activeBasarId);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => Cookie.GetCart()).MustHaveHappenedOnceExactly();
        A.CallTo(() => Cookie.ClearCart()).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionService.CheckoutAsync(activeBasarId, cart)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionRouter.ToSucess(saleId)).MustHaveHappenedOnceExactly();
    }
}
