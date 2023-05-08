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
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        VerifyNoOtherCalls();
    }
}
