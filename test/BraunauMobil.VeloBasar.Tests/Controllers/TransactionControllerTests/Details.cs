using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Details
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ReturnsView(int id)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.BuildTransaction().Create();
        A.CallTo(() => TransactionService.GetAsync(id)).Returns(transaction);

        //  Act
        IActionResult result = await Sut.Details(id);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(transaction);
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        A.CallTo(() => TransactionService.GetAsync(id)).MustHaveHappenedOnceExactly();
    }
}
