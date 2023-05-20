using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class Export
    : TestBase
{
    [Fact]
    public void ReturnsView()
    {
        //  Arrange

        //  Act
        IActionResult result = Sut.Export();

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        VerifyNoOtherCalls();
    }
}
