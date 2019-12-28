using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductsTransactionTests
{
    public class GetSoldProductsSum
    {
        [Fact]
        public void Test()
        {
            var tx = new ProductsTransaction
            {
                Products = new List<ProductToTransaction>
                {
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 123.12m,
                            StorageState = StorageState.Sold
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 99.99m,
                            StorageState = StorageState.Sold
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 100.0m,
                            StorageState = StorageState.Available
                        }
                    }
                }
            };
            Assert.Equal(223.11m, tx.GetSoldProductsSum());
        }
    }
}
