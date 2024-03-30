using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class Unsettle
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldCallUnsettleAndRedirecttoSuccess(int settlementId, int unsettlementId, string url)
    {
        //  Arrange
        TransactionEntity transaction = Fixture.Create<TransactionEntity>();
        A.CallTo(() => TransactionService.UnsettleAsync(settlementId)).Returns(unsettlementId);
        A.CallTo(() => TransactionRouter.ToSucess(unsettlementId)).Returns(url);

        //  Act
        IActionResult result = await Sut.Unsettle(settlementId);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => TransactionService.UnsettleAsync(settlementId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionRouter.ToSucess(unsettlementId)).MustHaveHappenedOnceExactly();
    }
}
