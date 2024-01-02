using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Cancel
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ReturnsRedirectToSelectProducts(int id, string url)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.Create<TransactionEntity>();
        A.CallTo(() => TransactionService.GetAsync(id)).Returns(transaction);
        A.CallTo(() => CancelRouter.ToSelectProducts(transaction.Id)).Returns(url);

        //  Act
        IActionResult result = await Sut.Cancel(id);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => TransactionService.GetAsync(id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => CancelRouter.ToSelectProducts(transaction.Id)).MustHaveHappenedOnceExactly();
    }
}
