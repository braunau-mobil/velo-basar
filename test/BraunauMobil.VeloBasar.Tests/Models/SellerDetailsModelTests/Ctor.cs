namespace BraunauMobil.VeloBasar.Tests.Models.SellerDetailsModelTests;

public class Ctor
{
    [Theory]
    [VeloAutoData]
    public void CheckDefaults(SellerEntity seller)
    {
        //  Arrange

        //  Act
        SellerDetailsModel sut = new(seller);

        //  Assert
        using (new AssertionScope())
        {
            sut.Entity.Should().Be(seller);
            sut.BasarId.Should().Be(0);
            sut.CanPushStatus.Should().BeFalse();
            sut.AcceptedProductCount.Should().Be(0);
            sut.SoldProductCount.Should().Be(0);
            sut.NotSoldProductCount.Should().Be(0);
            sut.PickedUpProductCount.Should().Be(0);
            sut.SettlementAmout.Should().Be(0);
            sut.Transactions.Should().BeEmpty();
            sut.Products.Should().BeEmpty();
        }
    }
}
