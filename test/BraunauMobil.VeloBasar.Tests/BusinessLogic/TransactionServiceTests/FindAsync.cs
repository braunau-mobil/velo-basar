using FluentAssertions.Execution;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class FindAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task Existing_ReturnsTransactionIncludingAllRelations(BasarEntity basar)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSessionEntity()
            .With(_ => _.Basar, basar)
            .Create();
        ProductEntity[] products = Fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.Available)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);

        TransactionEntity transaction = Fixture.BuildTransaction()
            .With(_ => _.Basar, basar)
            .Create();
        foreach (ProductEntity product in products)
        {
            transaction.Products.Add(new ProductToTransactionEntity(transaction, product));
        }
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        //  Act
        var x = Db.Transactions.ToArray();

        TransactionEntity? foundTransation = await Sut.FindAsync(basar.Id, transaction.Type, transaction.Number);

        //  Assert
        using (new AssertionScope())
        {
            foundTransation.Should().BeEquivalentTo(transaction);
        }
    }

    [Theory]
    [AutoData]
    public async Task Existing_SearchOther_ReturnsNull(BasarEntity basar)
    {
        // Arrange
        AcceptSessionEntity session = Fixture.BuildAcceptSessionEntity()
            .With(_ => _.Basar, basar)
            .Create();
        ProductEntity[] products = Fixture.BuildProductEntity()
            .With(_ => _.StorageState, StorageState.Available)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .With(_ => _.Session, session)
            .CreateMany().ToArray();
        Db.Products.AddRange(products);

        TransactionEntity transaction = Fixture.BuildTransaction()
            .With(_ => _.Basar, basar)
            .Create();
        foreach (ProductEntity product in products)
        {
            transaction.Products.Add(new ProductToTransactionEntity(transaction, product));
        }
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        //  Act
        TransactionEntity? foundTransation = await Sut.FindAsync(basar.Id, transaction.Type, transaction.Number + 1);

        //  Assert
        foundTransation.Should().BeNull();
    }

    [Theory]
    [AutoData]
    public async Task EmptyTable_ReturnsNull(int basarId, TransactionType transactionType, int number)
    {
        // Arrange

        //  Act
        TransactionEntity? foundTransation = await Sut.FindAsync(basarId, transactionType, number);

        //  Assert
        foundTransation.Should().BeNull();
    }
}
