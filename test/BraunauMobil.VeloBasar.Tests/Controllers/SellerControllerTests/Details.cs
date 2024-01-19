using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class Details
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ReturnsView(SellerDetailsParameter parameter)
    {
        //  Arrange
        SellerDetailsModel model = Fixture.Build<SellerDetailsModel>()
             .With(_ => _.Transactions, Fixture.BuildTransaction().CreateMany().ToList())
             .Create();
        A.CallTo(() => SellerService.GetDetailsAsync(parameter.BasarId, parameter.Id)).Returns(model);

        //  Act
        IActionResult result = await Sut.Details(parameter);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => SellerService.GetDetailsAsync(parameter.BasarId, parameter.Id)).MustHaveHappenedOnceExactly();
    }
}
