using FluentAssertions.Execution;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class DetailsAsync_SampleDb
    : TestBase<SampleSqliteDbFixture>
{
    [Theory]
    [InlineData(1, 12)]
    public async Task DetailsAreCorrect(int basarId, int sellerId)
    {
        //  Arrange
        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);

        //  Act
        SellerDetailsModel model = await Sut.GetDetailsAsync(basarId, sellerId);

        //  Assert
        using (new AssertionScope())
        {
            model.AcceptedProductCount.Should().Be(12);
            model.CanPushStatus.Should().BeTrue();
            model.Entity.Id.Should().Be(sellerId);
            model.Entity.ValueState.Should().Be(ValueState.Settled);
            model.NotSoldProductCount.Should().Be(5);
            model.PickedUpProductCount.Should().Be(1);
            model.Procucts.Should().HaveCount(12);
            model.SettlementAmout.Should().Be(590.8896M);
            model.SoldProductCount.Should().Be(7);
            model.Transactions.Should().HaveCount(4);
        }

        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
    }
}
