using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.ProductTests
{
    public class SetState
    {
        [Theory]
        [InlineData(TransactionType.Acceptance, StorageState.Available, ValueState.NotSettled)]
        [InlineData(TransactionType.Cancellation, StorageState.Available, ValueState.NotSettled)]
        [InlineData(TransactionType.Lock, StorageState.Locked, ValueState.NotSettled)]
        [InlineData(TransactionType.MarkAsGone, StorageState.Gone, ValueState.NotSettled)]
        [InlineData(TransactionType.Release, StorageState.Available, ValueState.NotSettled)]
        [InlineData(TransactionType.Sale, StorageState.Sold, ValueState.NotSettled)]
        [InlineData(TransactionType.Settlement, StorageState.Available, ValueState.Settled)]
        public void Test(TransactionType transactionType, StorageState expectedStorageState, ValueState expectedValueState)
        {
            var product = new Product();
            product.SetState(transactionType);

            Assert.Equal(expectedStorageState, product.StorageState);
            Assert.Equal(expectedValueState, product.ValueState);
        }
    }
}
