using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SetupControllerTests;

public class InitialSetup
    : TestBase
{
    [Fact]
    public void ReturnsView()
    {
        //  Arrange

        //  Act
        IActionResult result = Sut.InitialSetup();

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeOfType<InitializationConfiguration>();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }
    }
}
