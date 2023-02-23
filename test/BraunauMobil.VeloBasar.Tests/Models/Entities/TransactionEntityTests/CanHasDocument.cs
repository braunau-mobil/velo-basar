namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class CanHasDocument
{
    [Theory]
    [InlineData(TransactionType.Acceptance)]
    [InlineData(TransactionType.Sale)]
    [InlineData(TransactionType.Settlement)]
    public void Yes(TransactionType transactionType)
    {
        var transaction = new TransactionEntity
        {
            Type = transactionType
        };
        transaction.CanHasDocument
            .Should().BeTrue();
    }
    [Theory]
    [InlineData(TransactionType.Cancellation)]
    [InlineData(TransactionType.Lock)]
    [InlineData(TransactionType.SetLost)]
    [InlineData(TransactionType.Unlock)]
    public void No(TransactionType transactionType)
    {
        var transaction = new TransactionEntity
        {
            Type = transactionType
        };
        transaction.CanHasDocument
            .Should().BeFalse();
    }
}
