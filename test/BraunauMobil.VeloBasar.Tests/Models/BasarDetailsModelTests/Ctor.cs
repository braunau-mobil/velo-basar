namespace BraunauMobil.VeloBasar.Tests.Models.BasarDetailsModelTests;

public class Ctor
{
    [Theory]
    [VeloAutoData]
    public void CheckDefaults(BasarEntity basar, BasarSettlementStatus basarSettlementStatus)
    {
        //  Arrange

        //  Act
        BasarDetailsModel sut = new(basar, basarSettlementStatus);

        //  Assert
        using (new AssertionScope())
        {
            sut.Entity.Should().Be(basar);
            sut.AcceptanceCount.Should().Be(0);
            sut.AcceptedProductsAmount.Should().Be(0);
            sut.AcceptedProductsCount.Should().Be(0);
            sut.AcceptedProductTypesByAmount.Should().BeEmpty();
            sut.AcceptedProductTypesByCount.Should().BeEmpty();
            sut.LockedProductsCount.Should().Be(0);
            sut.LostProductsCount.Should().Be(0);
            sut.PriceDistribution.Should().BeEmpty();
            sut.SaleCount.Should().Be(0);
            sut.SaleDistribution.Should().BeEmpty();
            sut.SettlementStatus.Should().Be(basarSettlementStatus);
            sut.SoldProductsAmount.Should().Be(0);
            sut.SoldProductsCount.Should().Be(0);
            sut.SoldProductTypesByCount.Should().BeEmpty();
            sut.SoldProductTypesByAmount.Should().BeEmpty();
        }
    }
}
