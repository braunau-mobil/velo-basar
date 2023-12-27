using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Delete
    : TestBase
{
    [Theory]
    [AutoData]
    public void CallsRemoveAndReturnsRedirectToIndex(int productId)
    {
        //  Arrange
        List<int> cart = new();
        A.CallTo(() => Cookie.GetCart()).Returns(cart);
        A.CallTo(() => Cookie.SetCart(cart)).DoesNothing();

        //  Act
        IActionResult result = Sut.Delete(productId);

        //  Assert
        RedirectToActionResult redirectToAction = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectToAction.ActionName.Should().Be(nameof(CartController.Index));
        redirectToAction.ControllerName.Should().BeNull();

        A.CallTo(() => Cookie.GetCart()).MustHaveHappenedOnceExactly();
        A.CallTo(() => Cookie.SetCart(cart)).MustHaveHappenedOnceExactly();
        cart.Should().BeEmpty();
    }
}
