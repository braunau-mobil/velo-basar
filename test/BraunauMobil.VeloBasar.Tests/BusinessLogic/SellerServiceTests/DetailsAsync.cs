namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class DetailsAsync
    : TestBase<SampleSqliteDbFixture>
{
    [Theory]
    [InlineData(1, 12)]
    public async Task DetailsAreCorrect(int basarId, int sellerId)
    {
        //  Arrange

        //  Act
        SellerDetailsModel model = await Sut.GetDetailsAsync(basarId, sellerId);

        //  Assert
        model.AcceptedProductCount.Should().Be(3);
        model.Entity.Id.Should().Be(sellerId);
        model.Entity.ValueState.Should().Be(ValueState.NotSettled);
        model.NotSoldProductCount.Should().Be(1);
        model.PickedUpProductCount.Should().Be(0);
        model.Procucts.Should().HaveCount(3);
        model.SettlementAmout.Should().Be(181.65M);
        model.SoldProductCount.Should().Be(2);
        model.Transactions.Should().HaveCount(3);

        VerifyNoOtherCalls();
    }
}
