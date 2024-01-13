namespace BraunauMobil.VeloBasar.Tests.Models.BasarDetailsModelTests;

public class AcceptedProductTypesByAmount
{
    [Theory]
    [VeloAutoData]
    public void DefaultShouldBeEmpty(BasarEntity basar, BasarSettlementStatus basarSettlementStatus)
    {
        //  Arrange
        BasarDetailsModel sut = new(basar, basarSettlementStatus);

        //  Act
        IReadOnlyList<ChartDataPoint> result = sut.AcceptedProductTypesByAmount;

        //  Assert
        result.Should().BeEmpty();
    }
}
