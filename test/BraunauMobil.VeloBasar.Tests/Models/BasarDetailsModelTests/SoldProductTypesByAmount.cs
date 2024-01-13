namespace BraunauMobil.VeloBasar.Tests.Models.BasarDetailsModelTests;

public class SoldProductTypesByAmount
{
    [Theory]
    [VeloAutoData]
    public void DefaultShouldBeEmpty(BasarEntity basar, BasarSettlementStatus basarSettlementStatus)
    {
        //  Arrange
        BasarDetailsModel sut = new(basar, basarSettlementStatus);

        //  Act
        IReadOnlyList<ChartDataPoint> result = sut.SoldProductTypesByAmount;

        //  Assert
        result.Should().BeEmpty();
    }
}
