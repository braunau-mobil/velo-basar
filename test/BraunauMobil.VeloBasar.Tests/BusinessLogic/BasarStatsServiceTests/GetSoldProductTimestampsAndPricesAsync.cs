namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSoldProductTimestampsAndPricesAsync
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task NoSalesAtAll_ShouldReturnEmpty(int basarId)
    {
        //  Arrange

        //  Act
        IReadOnlyList<Tuple<TimeOnly, decimal>> result = await Sut.GetSoldProductTimestampsAndPricesAsync(basarId);

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public async Task WithSales_ShouldBeCorrect(BasarEntity basar)
    {
        //  Arrange
        Db.Basars.Add(basar);
        CreateSale(basar, new TimeOnly(05, 09, 00), CreateProduct);
        CreateSale(basar, new TimeOnly(07, 28, 00), CreateProduct, CreateProduct, CreateProduct, CreateProduct);
        CreateSale(basar, new TimeOnly(04, 37, 00), CreateProduct, CreateProduct, CreateProduct);
        CreateSale(basar, new TimeOnly(08, 57, 00), CreateProduct, CreateProduct);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyList<Tuple<TimeOnly, decimal>> result = await Sut.GetSoldProductTimestampsAndPricesAsync(1);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(10);
            result.Should().BeEquivalentTo([
                (new TimeOnly(05, 09, 00), 10M),
                (new TimeOnly(07, 28, 00), 10M),
                (new TimeOnly(07, 28, 00), 10M),
                (new TimeOnly(07, 28, 00), 10M),
                (new TimeOnly(07, 28, 00), 10M),
                (new TimeOnly(04, 37, 00), 10M),
                (new TimeOnly(04, 37, 00), 10M),
                (new TimeOnly(04, 37, 00), 10M),
                (new TimeOnly(08, 57, 00), 10M),
                (new TimeOnly(08, 57, 00), 10M),
                ]);
        }
    }

    private void CreateSale(BasarEntity basar, TimeOnly time, params Action<TransactionEntity>[] createProductActions)
    {
        TransactionEntity transaction = _fixture.BuildTransaction(basar)
            .With(transaction => transaction.Type, TransactionType.Sale)
            .With(transaction => transaction.TimeStamp, new DateTime(DateOnly.FromDateTime(X.FirstContactDay), time))
            .Create();
        foreach (Action<TransactionEntity> action in createProductActions)
        {
            action(transaction);
        }
        Db.Transactions.Add(transaction);
    }

    private void CreateProduct(TransactionEntity transaction)
    {
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.Price, 10M)
            .With(product => product.StorageState, StorageState.Sold)
            .Create();
        ProductToTransactionEntity productToTransactionEntity = new (){ Product = product, Transaction = transaction };
        transaction.Products.Add(productToTransactionEntity);
    }
}
