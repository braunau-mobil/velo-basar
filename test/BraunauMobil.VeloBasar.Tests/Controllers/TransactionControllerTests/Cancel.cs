using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Cancel
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task ReturnsRedirectToSelectProducts(int id, string url)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.BuildTransaction().Create();
        A.CallTo(() => TransactionService.GetAsync(id)).Returns(transaction);
        A.CallTo(() => CancelRouter.ToSelectProducts(transaction.Id)).Returns(url);

        //  Act
        IActionResult result = await Sut.Cancel(id);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        A.CallTo(() => TransactionService.GetAsync(id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => CancelRouter.ToSelectProducts(transaction.Id)).MustHaveHappenedOnceExactly();
    }
}
