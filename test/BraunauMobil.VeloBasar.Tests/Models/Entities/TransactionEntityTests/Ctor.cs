namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class Ctor
{
    [Fact]
    public void CheckDefaults()
    {
        //  Arrange

        //  Act
        TransactionEntity sut = new();

        //  Assert
        using (new AssertionScope())
        {
            sut.Basar.Should().BeNull();
            sut.BasarId.Should().Be(0);
            sut.Change.Should().BeNull();
            sut.DocumentId.Should().BeNull();
            sut.Id.Should().Be(0);
            sut.Notes.Should().BeNull();
            sut.Number.Should().Be(0);
            sut.ParentTransaction.Should().BeNull();
            sut.ParentTransactionId.Should().BeNull();
            sut.Products.Should().BeEmpty();
            sut.Seller.Should().BeNull();
            sut.Seller.Should().BeNull();
            sut.SellerId.Should().BeNull();
            sut.TimeStamp.Should().Be(DateTime.MinValue);
            sut.Type.Should().Be(TransactionType.Acceptance);
        }
    }
}
