using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class Details
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ReturnsView(int activeBasarId, int sellerId)
    {
        //  Arrange
        SellerDetailsModel model = Fixture.BuildSellerDetailsModel().Create();
        A.CallTo(() => SellerService.GetDetailsAsync(activeBasarId, sellerId)).Returns(model);

        //  Act
        IActionResult result = await Sut.Details(activeBasarId, sellerId);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => SellerService.GetDetailsAsync(activeBasarId, sellerId)).MustHaveHappenedOnceExactly();
    }
}
