using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.TransactionExtensionsTests;

public class WhereBasarAndSeller
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public void ShouldReturnOnlyForBasarAndOfType(int basarId, int sellerId)
    {
        // Arrange
        List<TransactionEntity> transactions =
        [
            .. _fixture.BuildTransaction()
                .With(transaction => transaction.BasarId, basarId)
                .With(transaction => transaction.SellerId, sellerId)
                .CreateMany(),
            .. _fixture.CreateMany<TransactionEntity>(),
        ];
        IQueryable<TransactionEntity> transactionsQueryable = transactions.AsQueryable();

        // Act
        var result = transactionsQueryable.WhereBasarAndSeller(basarId, sellerId);

        // Assert
        result.Should().AllSatisfy(transaction =>
        {
            transaction.BasarId.Should().Be(basarId);
            transaction.SellerId.Should().Be(sellerId);
        });
    }

}
