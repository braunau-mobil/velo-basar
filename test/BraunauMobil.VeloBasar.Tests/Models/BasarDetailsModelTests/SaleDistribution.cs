namespace BraunauMobil.VeloBasar.Tests.Models.BasarDetailsModelTests;

public class SaleDistribution
{
    [Theory]
    [VeloAutoData]
    public void DefaultShouldBeEmpty(BasarEntity basar, BasarSettlementStatus basarSettlementStatus)
    {
        //  Arrange
        BasarDetailsModel sut = new(basar, basarSettlementStatus);

        //  Act
        IReadOnlyList<ChartDataPoint> result = sut.SaleDistribution;

        //  Assert
        result.Should().BeEmpty();
    }
}
