using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<InitializationConfiguration>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();
    }
}
