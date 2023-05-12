using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.CartControllerTests;

public class Clear
    : TestBase
{
    [Fact]
    public void CallsClearCartAndReturnsRedirectToIndex()
    {
        //  Arrange

        //  Act
        IActionResult result = Sut.Clear();

        //  Assert
        result.Should().NotBeNull();
        RedirectToActionResult redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(CartController.Index));
        redirectResult.ControllerName.Should().BeNull();

        Cookie.Verify(_ => _.ClearCart(), Times.Once());
        VerifyNoOtherCalls();
    }
}
