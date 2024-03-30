namespace BraunauMobil.VeloBasar.Tests.Models.TransactionSuccessModelTests;

public class Ctor
{
    [Theory]
    [VeloAutoData]
    public void CheckDefaults(TransactionEntity transaction)
    {
        //  Arrange

        //  Act
        TransactionSuccessModel sut = new(transaction);

        //  Assert
        using (new AssertionScope())
        {
            sut.AmountGiven.Should().Be(0);
            sut.Entity.Should().Be(transaction);
            sut.OpenDocument.Should().BeFalse();
        }
    }

    [Theory]
    [VeloInlineAutoData(TransactionType.Cancellation)]
    public void ShouldUseIdFromParent(TransactionType type, TransactionEntity transaction, TransactionEntity parentTransaction)
    {
        //  Arrange
        transaction.ParentTransaction = parentTransaction;
        transaction.Type = type;

        //  Act
        TransactionSuccessModel sut = new(transaction, true);

        //  Assert
        using (new AssertionScope())
        {
            sut.AmountGiven.Should().Be(0);
            sut.Entity.Should().Be(transaction);
            sut.OpenDocument.Should().BeTrue();
            sut.DocumentTransactionId.Should().Be(transaction.ParentTransaction!.Id);
        }
    }


    [Fact]
    public void Other_ShouldUseDocumentIdTransaction()
    {
        //  Arrange
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(TransactionType.Cancellation);
        TransactionEntity transaction = fixture.Create<TransactionEntity>();

        //  Act
        TransactionSuccessModel sut = new(transaction);

        //  Assert
        using (new AssertionScope())
        {
            sut.AmountGiven.Should().Be(0);
            sut.Entity.Should().Be(transaction);
            sut.OpenDocument.Should().BeFalse();
            sut.DocumentTransactionId.Should().Be(transaction.Id);
        }
    }
}
