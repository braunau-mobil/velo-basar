namespace BraunauMobil.VeloBasar.Tests.Models.BasarDetailsModelTests;

public class SoldProductTypesByCount
{
    [Theory]
    [VeloAutoData]
    public void DefaultShouldBeEmpty(BasarEntity basar, BasarSettlementStatus basarSettlementStatus)
    {
        //  Arrange
        BasarDetailsModel sut = new(basar, basarSettlementStatus);

        //  Act
        IReadOnlyList<ChartDataPoint> result = sut.SoldProductTypesByCount;

        //  Assert
        result.Should().BeEmpty();
    }
}
