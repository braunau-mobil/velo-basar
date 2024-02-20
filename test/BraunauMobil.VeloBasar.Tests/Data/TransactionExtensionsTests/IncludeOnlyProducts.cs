using BraunauMobil.VeloBasar.Data;
using Xan.AspNetCore.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.Data.TransactionExtensionsTests;

public class IncludeOnlyProducts
    : DbTestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldLoadOnlyProductsAndTheirRelations(TransactionEntity transaction, TransactionEntity parentTransaction, TransactionEntity[] childTransactions, SellerEntity seller, ProductEntity[] products)
    {
        //  Arrange
        transaction.Seller = seller;
        transaction.ParentTransaction = parentTransaction;
        foreach (TransactionEntity childTransaction in childTransactions)
        {
            transaction.ChildTransactions.Add(childTransaction);
        }
        foreach (ProductEntity product in products)
        {
            transaction.Products.Add(new ProductToTransactionEntity { Transaction = transaction, Product = product });
        }
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();
        Db.ChangeTracker.Clear();

        //  Act
        TransactionEntity result = await Db.Transactions.IncludeOnlyProducts().FirstByIdAsync(transaction.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.ParentTransaction.Should().BeNull();
            result.ChildTransactions.Should().BeEmpty();
            result.Basar.Should().BeNull();
            result.Seller.Should().BeNull();
            result.Products.Should().NotBeEmpty();
            result.Products.Should().AllSatisfy(productToTransaction =>
            {
                productToTransaction.Product.Session.Should().NotBeNull();
                productToTransaction.Product.Session.Seller.Should().NotBeNull();
                productToTransaction.Product.Session.Basar.Should().NotBeNull();
                productToTransaction.Product.Type.Should().NotBeNull();
            });
        }
    }
}
