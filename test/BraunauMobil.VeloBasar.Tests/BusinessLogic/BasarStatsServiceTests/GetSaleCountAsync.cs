using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSaleCountAsync
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();

    [Theory]
    [AutoData]
    public async Task NoTransactionsAtAll_ShouldReturnZero(int basarId)
    {
        //  Arrange

        //  Act
        int count = await Sut.GetSaleCountAsync(basarId);

        //  Assert
        count.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task BasarHasNoTransactions_ShouldReturnZero(BasarEntity basar)
    {
        //  Arrange
        Db.Basars.Add(basar);
        IEnumerable<TransactionEntity> otherTransactions = _fixture.BuildTransaction().CreateMany();
        Db.Transactions.AddRange(otherTransactions);
        await Db.SaveChangesAsync();

        //  Act
        int count = await Sut.GetSaleCountAsync(basar.Id);

        //  Assert
        count.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task BasarHasTransactions_ShouldReturnCount(BasarEntity basar, BasarEntity otherBasar)
    {
        //  Arrange
        IEnumerable<TransactionEntity> basarAcceptances = _fixture
            .BuildTransaction()
            .WithBasar(basar)
            .With(_ => _.Type, TransactionType.Sale)
            .CreateMany();
        Db.Transactions.AddRange(basarAcceptances);

        IEnumerable<TransactionEntity> otherTransactions = _fixture.BuildTransaction()
            .WithBasar(otherBasar)
            .CreateMany();
        Db.Transactions.AddRange(otherTransactions);
        await Db.SaveChangesAsync();

        //  Act
        int count = await Sut.GetSaleCountAsync(basar.Id);

        //  Assert
        count.Should().Be(basarAcceptances.Count());

        VerifyNoOtherCalls();
    }
}
