namespace BraunauMobil.VeloBasar.Tests.Models.BasarDetailsModelTests;

public class PriceDistribution
{
    [Theory]
    [VeloAutoData]
    public void DefaultShouldBeEmpty(BasarEntity basar, BasarSettlementStatus basarSettlementStatus)
    {
        //  Arrange
        BasarDetailsModel sut = new(basar, basarSettlementStatus);

        //  Act
        IReadOnlyList<ChartDataPoint> result = sut.PriceDistribution;

        //  Assert
        result.Should().BeEmpty();
    }
}
