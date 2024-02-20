namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetDetailsAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task EmptyDatabase_Throws( int productId)
    {
        //  Arrange

        //  Act
        Func<Task> act = async () => await Sut.GetDetailsAsync(productId);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductExistsWithNoTransactions_ReturnsDetailsModel(ProductEntity product)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();

        //  Act
        ProductDetailsModel model = await Sut.GetDetailsAsync(product.Id);

        //  Assert
        using (new AssertionScope())
        {
            model.Entity.Should().BeEquivalentTo(product);
            model.Transactions.Should().BeEmpty();
            model.CanLock.Should().BeFalse();
            model.CanSetAsLost.Should().BeFalse();
            model.CanUnlock.Should().BeFalse();
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductAndTransactionsExist_ReturnsDetailsModel(ProductEntity product)
    {
        //  Arrange
        Db.Products.Add(product);
        await Db.SaveChangesAsync();
        TransactionEntity transaction = new ()
        {
            Basar = product.Session.Basar,
            Type = TransactionType.Acceptance,
            Seller = product.Session.Seller
        };
        transaction.Products.Clear();
        transaction.Products.Add(new ProductToTransactionEntity
        {
            Transaction = transaction,
            Product = product
        });
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        //  Act
        ProductDetailsModel model = await Sut.GetDetailsAsync(product.Id);

        //  Assert
        using (new AssertionScope())
        {
            model.Entity.Should().BeEquivalentTo(product);
            model.Transactions.Should().ContainEquivalentOf(transaction);
            model.CanLock.Should().BeFalse();
            model.CanSetAsLost.Should().BeFalse();
            model.CanUnlock.Should().BeFalse();
        }
    }
}
