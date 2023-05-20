using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Delete
    : TestBase
{
    [Theory]
    [AutoData]
    public void CallsRemoveAndReturnsRedirectToIndex(int productId)
    {
        //  Arrange
        Mock<IList<int>> cartMock = new ();
        Cookie.Setup(_ => _.GetCart())
            .Returns(cartMock.Object);

        //  Act
        IActionResult result = Sut.Delete(productId);

        //  Assert
        RedirectToActionResult redirectToAction = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectToAction.ActionName.Should().Be(nameof(CartController.Index));
        redirectToAction.ControllerName.Should().BeNull();

        Cookie.Verify(_ => _.GetCart(), Times.Once());
        Cookie.Verify(_ => _.SetCart(cartMock.Object), Times.Once());
        cartMock.Verify(_ => _.Remove(productId), Times.Once());
        VerifyNoOtherCalls();
    }
}
