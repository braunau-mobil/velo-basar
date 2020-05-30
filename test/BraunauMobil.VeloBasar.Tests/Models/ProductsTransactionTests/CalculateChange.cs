using AutoFixture;
using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductsTransactionTests
{
    public class CalculateChange
    {
        [Theory]
        [InlineData(87.99, 87.99)]
        public void Cancellation(decimal price, decimal changeAmount)
        {
            var settings = new VeloSettings();

            var fixture = new Fixture();
            var product = fixture.BuildProduct(price).Create();
            product.StorageState = StorageState.Available;
            var sale = new ProductsTransaction
            {
                Type = TransactionType.Cancellation,
                Products = new[]
                {
                    new ProductToTransaction
                    {
                        Product = product
                    }
                }
            };

            var changeInfo = sale.CalculateChange(0.0m, settings.Nominations);
            Assert.Equal(changeAmount, changeInfo.Amount);
        }
        [Theory]
        [InlineData(87.99, 100, 12.01)]
        public void Sale(decimal price, decimal amountGiven, decimal changeAmount)
        {
            var settings = new VeloSettings();

            var fixture = new Fixture();
            var product = fixture.BuildProduct(price).Create();
            product.StorageState = StorageState.Sold;
            var sale = new ProductsTransaction
            {
                Type = TransactionType.Sale,
                Products = new []
                {
                    new ProductToTransaction
                    {
                        Product = product
                    }
                }
            };

            var changeInfo = sale.CalculateChange(amountGiven, settings.Nominations);
            Assert.Equal(changeAmount, changeInfo.Amount);
        }
        [Theory]
        [InlineData(87.99, 79.191)]
        public void SettlementWithTenPercentCommission(decimal price, decimal changeAmount)
        {
            var settings = new VeloSettings();

            var fixture = new Fixture();
            var soldProduct = fixture.BuildProduct(price).Create();
            soldProduct.StorageState = StorageState.Sold;
            var notSoldProduct = fixture.BuildProduct(price).Create();
            notSoldProduct.StorageState = StorageState.Available;
            var settlement = new ProductsTransaction
            {
                Basar = new Basar
                {
                    ProductCommissionPercentage = 10
                },
                Type = TransactionType.Settlement,
                Products = new[]
                {
                    new ProductToTransaction
                    {
                        Product = soldProduct
                    },
                    new ProductToTransaction
                    {
                        Product = notSoldProduct
                    }
                }
            };

            var changeInfo = settlement.CalculateChange(0.0m, settings.Nominations);
            Assert.Equal(changeAmount, changeInfo.Amount);
        }
    }
}
