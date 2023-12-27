using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class PrintTest
    : TestBase
{
    [Fact]
    public void ReturnsView()
    {
        //  Arrange

        //  Act
        IActionResult result = Sut.PrintTest();

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }
    }
}
