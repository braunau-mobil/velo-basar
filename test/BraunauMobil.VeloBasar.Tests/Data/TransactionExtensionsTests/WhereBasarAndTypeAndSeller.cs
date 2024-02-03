using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.TransactionExtensionsTests;

public class WhereBasarAndTypeAndSeller
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public void ShouldReturnOnlyForBasarAndOfType(int basarId, TransactionType transactionType, int sellerId)
    {
        // Arrange
        List<TransactionEntity> transactions =
        [
            .. _fixture.BuildTransaction()
                .With(transaction => transaction.BasarId, basarId)
                .With(transaction => transaction.Type, transactionType)
                .With(transaction => transaction.SellerId, sellerId)
                .CreateMany(),
            .. _fixture.CreateMany<TransactionEntity>(),
        ];
        IQueryable<TransactionEntity> transactionsQueryable = transactions.AsQueryable();

        // Act
        var result = transactionsQueryable.WhereBasarAndTypeAndSeller(basarId, transactionType, sellerId);

        // Assert
        result.Should().AllSatisfy(transaction =>
        {
            transaction.BasarId.Should().Be(basarId);
            transaction.Type.Should().Be(transactionType);
            transaction.SellerId.Should().Be(sellerId);
        });
    }

}
