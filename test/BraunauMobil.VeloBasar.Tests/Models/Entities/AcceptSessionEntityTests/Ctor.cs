namespace BraunauMobil.VeloBasar.Tests.Models.Entities.AcceptSessionEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        AcceptSessionEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Basar.Should().BeNull();
            sut.BasarId.Should().Be(0);
            sut.EndTimeStamp.Should().BeNull();
            sut.IsCompleted.Should().BeFalse();
            sut.Products.Should().BeEmpty();
            sut.Seller.Should().BeNull();
            sut.SellerId.Should().Be(0);
            sut.StartTimeStamp.Should().Be(DateTime.MinValue);
            sut.State.Should().Be(AcceptSessionState.Uncompleted);
        }
    }

    [Theory]
    [VeloAutoData]
    public void BasarIdSellerAndTimeStamp(int basarId, SellerEntity seller, DateTime timestamp)
    {
        //  Arrange

        //  Act
        AcceptSessionEntity sut = new(basarId, seller, timestamp);

        //  Assert
        using (new AssertionScope())
        {
            sut.Basar.Should().BeNull();
            sut.BasarId.Should().Be(basarId);
            sut.EndTimeStamp.Should().BeNull();
            sut.IsCompleted.Should().BeFalse();
            sut.Products.Should().BeEmpty();
            sut.Seller.Should().Be(seller);
            sut.SellerId.Should().Be(seller.Id);
            sut.StartTimeStamp.Should().Be(timestamp);
            sut.State.Should().Be(AcceptSessionState.Uncompleted);
        }
    }
}
