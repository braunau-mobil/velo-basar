using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductsTransactionTests
{
    public class CanRevert
    {
        [Theory]
        [InlineData(TransactionType.Settlement)]
        public void Allow(TransactionType type)
        {
            var tx = new ProductsTransaction
            {
                Type = type
            };

            Assert.True(tx.CanRevert());
        }
        [Theory]
        [InlineData(TransactionType.Acceptance)]
        [InlineData(TransactionType.Cancellation)]
        [InlineData(TransactionType.Lock)]
        [InlineData(TransactionType.MarkAsGone)]
        [InlineData(TransactionType.Release)]
        [InlineData(TransactionType.Sale)]

        public void Disallow(TransactionType type)
        {
            var tx = new ProductsTransaction
            {
                Type = type
            };

            Assert.False(tx.CanRevert());
        }
    }
}
