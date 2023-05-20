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
        TransactionService.Setup(_ => _.GetAsync(id))
            .ReturnsAsync(transaction);
        CancelRouter.Setup(_ => _.ToSelectProducts(transaction.Id))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Cancel(id);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        TransactionService.Verify(_ => _.GetAsync(id), Times.Once);
        CancelRouter.Verify(_ => _.ToSelectProducts(transaction.Id), Times.Once);
        VerifyNoOtherCalls();
    }
}
