namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSettlementPercentage
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(25, 0, 0)]
    [InlineData(45, 23, 51)]
    [InlineData(56, 56, 100)]
    public void ReturnsPercentage(int sellerCount, int settledSellerCount, int expectedPercentage)
    {
        //  Arrange

        //  Act
        int actualPercentage = Sut.GetSettlementPercentage(sellerCount, settledSellerCount);

        //  Assert
        Assert.Equal(expectedPercentage, actualPercentage);

        VerifyNoOtherCalls();
    }
}
