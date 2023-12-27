using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AdminControllerTests;

public class Export
    : TestBase
{
    [Theory]
    [AutoData]
    public void ReturnsView(DateTime dateTime)
    {
        //  Arrange
        Clock.Now = dateTime;

        //  Act
        IActionResult result = Sut.Export();

        //  Assert
        using(new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewData.ModelState.IsValid.Should().BeTrue();

            ExportModel model = view.Model.Should().BeOfType<ExportModel>().Subject;
            model.MinPermissionDate.Should().Be(DateOnly.FromDateTime(dateTime.Date));
        }
    }
}
