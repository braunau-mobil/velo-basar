using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Base;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.Models.Base.TransactionModelTests
{
    public class CanHasDocument
    {
        [Theory]
        [InlineData(TransactionType.Acceptance)]
        [InlineData(TransactionType.Sale)]
        [InlineData(TransactionType.Settlement)]
        public void Yes(TransactionType transactionType)
        {
            var transaction = new TransactionModel
            {
                Type = transactionType
            };
            Assert.True(transaction.CanHasDocument());
        }
        [Theory]
        [InlineData(TransactionType.Cancellation)]
        [InlineData(TransactionType.Lock)]
        [InlineData(TransactionType.MarkAsGone)]
        [InlineData(TransactionType.Release)]
        public void No(TransactionType transactionType)
        {
            var transaction = new TransactionModel
            {
                Type = transactionType
            };
            Assert.False(transaction.CanHasDocument());
        }
    }
}
