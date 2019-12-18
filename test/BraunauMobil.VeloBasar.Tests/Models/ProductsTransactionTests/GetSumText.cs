using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductsTransactionTests
{
    public class GetSumText
    {
        [Fact]
        public void TwoProducts()
        {
            var transaction = new ProductsTransaction
            {
                Products = new List<ProductToTransaction>
                {
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 123.12m
                        }
                    },
                    new ProductToTransaction
                    {
                        Product = new Product
                        {
                            Price = 66.55m
                        }
                    }
                }
            };
            Assert.Equal("189,67 €", transaction.GetSumText());
        }
    }
}
