namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetAcceptanceCountAsync
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task NoTransactionsAtAll_ShouldReturnZero(int basarId)
    {
        //  Arrange

        //  Act
        int count = await Sut.GetAcceptanceCountAsync(basarId);

        //  Assert
        count.Should().Be(0);
    }

    [Theory]
    [VeloAutoData]
    public async Task BasarHasNoTransactions_ShouldReturnZero(BasarEntity basar)
    {
        //  Arrange
        IEnumerable<TransactionEntity> otherTransactions = _fixture.CreateMany<TransactionEntity>();
        Db.Transactions.AddRange(otherTransactions);
        await Db.SaveChangesAsync();

        //  Act
        int count = await Sut.GetAcceptanceCountAsync(basar.Id);

        //  Assert
        count.Should().Be(0);
    }

    [Theory]
    [VeloAutoData]
    public async Task BasarHasTransactions_ShouldReturnCount(BasarEntity basar, BasarEntity otherBasar)
    {
        //  Arrange
        IEnumerable<TransactionEntity> basarAcceptances = _fixture
            .BuildTransaction(basar)
            .With(_ => _.Type, TransactionType.Acceptance)
            .CreateMany();
        Db.Transactions.AddRange(basarAcceptances);

        IEnumerable<TransactionEntity> otherTransactions = _fixture
            .BuildTransaction(otherBasar)
            .CreateMany();
        Db.Transactions.AddRange(otherTransactions);
        await Db.SaveChangesAsync();

        //  Act
        int count = await Sut.GetAcceptanceCountAsync(basar.Id);

        //  Assert
        count.Should().Be(basarAcceptances.Count());
    }
}
