using BraunauMobil.VeloBasar.Models;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class IsAllowed
    {
        [Theory]
        [InlineData(StorageState.Available, TransactionType.Lock)]
        [InlineData(StorageState.Available, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Available, TransactionType.Sale)]
        [InlineData(StorageState.Available, TransactionType.Settlement)]
        [InlineData(StorageState.Gone, TransactionType.Release)]
        [InlineData(StorageState.Locked, TransactionType.Release)]
        [InlineData(StorageState.Sold, TransactionType.Cancellation)]
        [InlineData(StorageState.Sold, TransactionType.Lock)]
        public void NotSettledAllow(StorageState storageState, TransactionType transactionType)
        {
            var product = new Product
            {
                StorageState = storageState,
                ValueState = ValueState.NotSettled
            };
            Assert.True(product.IsAllowed(transactionType));
        }
        [Theory]
        [InlineData(StorageState.Available, TransactionType.Acceptance)]
        [InlineData(StorageState.Available, TransactionType.Cancellation)]
        [InlineData(StorageState.Available, TransactionType.Release)]
        [InlineData(StorageState.Gone, TransactionType.Acceptance)]
        [InlineData(StorageState.Gone, TransactionType.Cancellation)]
        [InlineData(StorageState.Gone, TransactionType.Lock)]
        [InlineData(StorageState.Gone, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Gone, TransactionType.Sale)]
        [InlineData(StorageState.Gone, TransactionType.Settlement)]
        [InlineData(StorageState.Locked, TransactionType.Acceptance)]
        [InlineData(StorageState.Locked, TransactionType.Cancellation)]
        [InlineData(StorageState.Locked, TransactionType.Lock)]
        [InlineData(StorageState.Locked, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Locked, TransactionType.Sale)]
        [InlineData(StorageState.Locked, TransactionType.Settlement)]
        [InlineData(StorageState.Sold, TransactionType.Acceptance)]
        [InlineData(StorageState.Sold, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Sold, TransactionType.Release)]
        [InlineData(StorageState.Sold, TransactionType.Sale)]
        public void NotSettledDisallow(StorageState storageState, TransactionType transactionType)
        {
            var product = new Product
            {
                StorageState = storageState,
                ValueState = ValueState.NotSettled
            };
            Assert.False(product.IsAllowed(transactionType));
        }
        [Theory]
        [InlineData(StorageState.Available, TransactionType.Acceptance)]
        [InlineData(StorageState.Available, TransactionType.Cancellation)]
        [InlineData(StorageState.Available, TransactionType.Lock)]
        [InlineData(StorageState.Available, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Available, TransactionType.Release)]
        [InlineData(StorageState.Available, TransactionType.Sale)]
        [InlineData(StorageState.Available, TransactionType.Settlement)]
        [InlineData(StorageState.Gone, TransactionType.Acceptance)]
        [InlineData(StorageState.Gone, TransactionType.Cancellation)]
        [InlineData(StorageState.Gone, TransactionType.Lock)]
        [InlineData(StorageState.Gone, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Gone, TransactionType.Release)]
        [InlineData(StorageState.Gone, TransactionType.Sale)]
        [InlineData(StorageState.Gone, TransactionType.Settlement)]
        [InlineData(StorageState.Locked, TransactionType.Acceptance)]
        [InlineData(StorageState.Locked, TransactionType.Cancellation)]
        [InlineData(StorageState.Locked, TransactionType.Lock)]
        [InlineData(StorageState.Locked, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Locked, TransactionType.Release)]
        [InlineData(StorageState.Locked, TransactionType.Sale)]
        [InlineData(StorageState.Locked, TransactionType.Settlement)]
        [InlineData(StorageState.Sold, TransactionType.Acceptance)]
        [InlineData(StorageState.Sold, TransactionType.Cancellation)]
        [InlineData(StorageState.Sold, TransactionType.Lock)]
        [InlineData(StorageState.Sold, TransactionType.MarkAsGone)]
        [InlineData(StorageState.Sold, TransactionType.Release)]
        [InlineData(StorageState.Sold, TransactionType.Sale)]
        [InlineData(StorageState.Sold, TransactionType.Settlement)]
        public void SettledDisallow(StorageState storageState, TransactionType transactionType)
        {
            var product = new Product
            {
                StorageState = storageState,
                ValueState = ValueState.Settled
            };
            Assert.False(product.IsAllowed(transactionType));
        }
    }
}
