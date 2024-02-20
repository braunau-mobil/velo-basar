using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.TransactionExtensionsTests;

public class GetTimestampAsync
    : DbTestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnTimestampOfTransaction(TransactionEntity transaction)
    {
        //  Arrange
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        //  Act
        DateTime result = await Db.Transactions.GetTimestampAsync(transaction.Id);

        //  Assert
        result.Should().Be(transaction.TimeStamp);
    }
}
