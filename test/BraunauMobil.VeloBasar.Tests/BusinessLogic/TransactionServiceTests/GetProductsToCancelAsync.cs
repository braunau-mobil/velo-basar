namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class GetProductsToCancelAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnProductsWhereCancellationIsAllowed(TransactionEntity transaction)
    {
        //  Arrange
        ProductEntity[] productsToCancel = Fixture.BuildProduct()
            .With(product => product.StorageState, StorageState.Sold)
            .With(product => product.ValueState, ValueState.NotSettled)
            .CreateMany().ToArray();
        Fixture.ExcludeEnumValues(StorageState.Sold);
        Fixture.ExcludeEnumValues(ValueState.NotSettled);
        ProductEntity[] otherProducts = Fixture.CreateMany<ProductEntity>().ToArray();

        foreach (ProductEntity product in productsToCancel)
        {
            transaction.Products.Add(new ProductToTransactionEntity { Product = product, Transaction = transaction });
        }
        foreach (ProductEntity product in otherProducts)
        {
            transaction.Products.Add(new ProductToTransactionEntity { Product = product, Transaction = transaction });
        }
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyList<ProductEntity> result = await Sut.GetProductsToCancelAsync(transaction.Id);

        //  Assert
        result.Should().BeEquivalentTo(productsToCancel);
    }
}
