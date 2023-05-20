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
        RedirectToActionResult redirect = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirect.ActionName.Should().Be(nameof(CartController.Index));
        redirect.ControllerName.Should().BeNull();

        Cookie.Verify(_ => _.ClearCart(), Times.Once());
        VerifyNoOtherCalls();
    }
}
