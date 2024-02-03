namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class GetAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ById_ShouldReturnTransactionWithIdAndChangeShouldBeZero(TransactionEntity toGet, TransactionEntity[] others)
    {
        //  Arrange
        Db.Transactions.Add(toGet);
        Db.Transactions.AddRange(others);
        await Db.SaveChangesAsync();

        //  Act
        TransactionEntity result = await Sut.GetAsync(toGet.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(toGet);
            result.Change.Amount.Should().Be(0);
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ByIdWithAmount_ShouldReturnTransactionWithIdAndChangeShouldBeAmountGiven(TransactionEntity toGet, decimal amountGiven, TransactionEntity[] others)
    {
        //  Arrange
        toGet.Type = TransactionType.Settlement;
        Db.Transactions.Add(toGet);
        Db.Transactions.AddRange(others);
        await Db.SaveChangesAsync();

        //  Act
        TransactionEntity result = await Sut.GetAsync(toGet.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(toGet);
            result.Change.Amount.Should().Be(toGet.GetPayoutAmount());
        }
    }
}
