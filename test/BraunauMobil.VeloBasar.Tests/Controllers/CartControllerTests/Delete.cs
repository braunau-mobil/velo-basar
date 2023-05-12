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
        Mock<IList<int>> cartMock = new Mock<IList<int>>();
        Cookie.Setup(_ => _.GetCart())
            .Returns(cartMock.Object);

        //  Act
        IActionResult result = Sut.Delete(productId);

        //  Assert
        result.Should().NotBeNull();
        RedirectToActionResult redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(CartController.Index));
        redirectResult.ControllerName.Should().BeNull();

        Cookie.Verify(_ => _.GetCart(), Times.Once());
        Cookie.Verify(_ => _.SetCart(cartMock.Object), Times.Once());
        cartMock.Verify(_ => _.Remove(productId), Times.Once());
        VerifyNoOtherCalls();
    }
}
