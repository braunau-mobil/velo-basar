namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class GetLatestForProductAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnLatestTransactionForProduct(ProductEntity product, TransactionEntity first, TransactionEntity second, TransactionEntity latest)
    {
        //  Arrange
        first.TimeStamp = X.FirstContactDay.AddSeconds(1);
        first.Products.Add(new ProductToTransactionEntity { Product = product, Transaction = first });
        second.TimeStamp = X.FirstContactDay.AddSeconds(2);
        second.Products.Add(new ProductToTransactionEntity { Product = product, Transaction = second });
        latest.TimeStamp = X.FirstContactDay.AddSeconds(3);
        latest.Products.Add(new ProductToTransactionEntity { Product = product, Transaction = latest });

        Db.Transactions.AddRange(first, second, latest);
        await Db.SaveChangesAsync();

        //  Act
        TransactionEntity result = await Sut.GetLatestForProductAsync(product.Id);

        //  Assert
        result.Should().BeEquivalentTo(latest);        
    }
}
