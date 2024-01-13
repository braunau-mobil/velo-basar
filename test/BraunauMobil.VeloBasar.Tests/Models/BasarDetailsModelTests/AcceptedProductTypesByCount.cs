namespace BraunauMobil.VeloBasar.Tests.Models.BasarDetailsModelTests;

public class AcceptedProductTypesByCount
{
    [Theory]
    [VeloAutoData]
    public void DefaultShouldBeEmpty(BasarEntity basar, BasarSettlementStatus basarSettlementStatus)
    {
        //  Arrange
        BasarDetailsModel sut = new(basar, basarSettlementStatus);

        //  Act
        IReadOnlyList<ChartDataPoint> result = sut.AcceptedProductTypesByCount;

        //  Assert
        result.Should().BeEmpty();
    }
}
