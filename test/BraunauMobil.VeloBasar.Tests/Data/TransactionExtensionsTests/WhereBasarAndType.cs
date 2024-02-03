using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.TransactionExtensionsTests;

public class WhereBasarAndType
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public void ShouldReturnOnlyForBasarAndOfType(int basarId, TransactionType transactionType)
    {
        // Arrange
        List<TransactionEntity> transactions =
        [
            .. _fixture.BuildTransaction()
                .With(transaction => transaction.BasarId, basarId)
                .With(transaction => transaction.Type, transactionType)
                .CreateMany(),
            .. _fixture.CreateMany<TransactionEntity>(),
        ];
        IQueryable<TransactionEntity> transactionsQueryable = transactions.AsQueryable();

        // Act
        var result = transactionsQueryable.WhereBasarAndType(basarId, transactionType);

        // Assert
        result.Should().AllSatisfy(transaction =>
        {
            transaction.BasarId.Should().Be(basarId);
            transaction.Type.Should().Be(transactionType);
        });
    }

}
