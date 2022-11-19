namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class SetState
{
    [Theory]

    [InlineData(TransactionType.Acceptance, StorageState.Available, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.Locked, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.Lost, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.NotAccepted, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.Sold, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.Available, ValueState.Settled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.Locked, ValueState.Settled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.Lost, ValueState.Settled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.NotAccepted, ValueState.Settled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Acceptance, StorageState.Sold, ValueState.Settled, StorageState.Available, ValueState.NotSettled)]

    [InlineData(TransactionType.Cancellation, StorageState.Available, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.Locked, ValueState.NotSettled, StorageState.Locked, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.Lost, ValueState.NotSettled, StorageState.Lost, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.NotAccepted, ValueState.NotSettled, StorageState.NotAccepted, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.Sold, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.Available, ValueState.Settled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.Locked, ValueState.Settled, StorageState.Locked, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.Lost, ValueState.Settled, StorageState.Lost, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.NotAccepted, ValueState.Settled, StorageState.NotAccepted, ValueState.NotSettled)]
    [InlineData(TransactionType.Cancellation, StorageState.Sold, ValueState.Settled, StorageState.Available, ValueState.NotSettled)]

    [InlineData(TransactionType.Lock, StorageState.Available, ValueState.NotSettled, StorageState.Locked, ValueState.NotSettled)]
    [InlineData(TransactionType.Lock, StorageState.Locked, ValueState.NotSettled, StorageState.Locked, ValueState.NotSettled)]
    [InlineData(TransactionType.Lock, StorageState.Lost, ValueState.NotSettled, StorageState.Locked, ValueState.NotSettled)]
    [InlineData(TransactionType.Lock, StorageState.NotAccepted, ValueState.NotSettled, StorageState.Locked, ValueState.NotSettled)]
    [InlineData(TransactionType.Lock, StorageState.Sold, ValueState.NotSettled, StorageState.Locked, ValueState.NotSettled)]
    [InlineData(TransactionType.Lock, StorageState.Available, ValueState.Settled, StorageState.Locked, ValueState.Settled)]
    [InlineData(TransactionType.Lock, StorageState.Locked, ValueState.Settled, StorageState.Locked, ValueState.Settled)]
    [InlineData(TransactionType.Lock, StorageState.Lost, ValueState.Settled, StorageState.Locked, ValueState.Settled)]
    [InlineData(TransactionType.Lock, StorageState.NotAccepted, ValueState.Settled, StorageState.Locked, ValueState.Settled)]
    [InlineData(TransactionType.Lock, StorageState.Sold, ValueState.Settled, StorageState.Locked, ValueState.Settled)]

    [InlineData(TransactionType.Sale, StorageState.Available, ValueState.NotSettled, StorageState.Sold, ValueState.NotSettled)]
    [InlineData(TransactionType.Sale, StorageState.Locked, ValueState.NotSettled, StorageState.Sold, ValueState.NotSettled)]
    [InlineData(TransactionType.Sale, StorageState.Lost, ValueState.NotSettled, StorageState.Sold, ValueState.NotSettled)]
    [InlineData(TransactionType.Sale, StorageState.NotAccepted, ValueState.NotSettled, StorageState.Sold, ValueState.NotSettled)]
    [InlineData(TransactionType.Sale, StorageState.Sold, ValueState.NotSettled, StorageState.Sold, ValueState.NotSettled)]
    [InlineData(TransactionType.Sale, StorageState.Available, ValueState.Settled, StorageState.Sold, ValueState.Settled)]
    [InlineData(TransactionType.Sale, StorageState.Locked, ValueState.Settled, StorageState.Sold, ValueState.Settled)]
    [InlineData(TransactionType.Sale, StorageState.Lost, ValueState.Settled, StorageState.Sold, ValueState.Settled)]
    [InlineData(TransactionType.Sale, StorageState.NotAccepted, ValueState.Settled, StorageState.Sold, ValueState.Settled)]
    [InlineData(TransactionType.Sale, StorageState.Sold, ValueState.Settled, StorageState.Sold, ValueState.Settled)]

    [InlineData(TransactionType.SetLost, StorageState.Available, ValueState.NotSettled, StorageState.Lost, ValueState.NotSettled)]
    [InlineData(TransactionType.SetLost, StorageState.Locked, ValueState.NotSettled, StorageState.Lost, ValueState.NotSettled)]
    [InlineData(TransactionType.SetLost, StorageState.Lost, ValueState.NotSettled, StorageState.Lost, ValueState.NotSettled)]
    [InlineData(TransactionType.SetLost, StorageState.NotAccepted, ValueState.NotSettled, StorageState.Lost, ValueState.NotSettled)]
    [InlineData(TransactionType.SetLost, StorageState.Sold, ValueState.NotSettled, StorageState.Lost, ValueState.NotSettled)]
    [InlineData(TransactionType.SetLost, StorageState.Available, ValueState.Settled, StorageState.Lost, ValueState.Settled)]
    [InlineData(TransactionType.SetLost, StorageState.Locked, ValueState.Settled, StorageState.Lost, ValueState.Settled)]
    [InlineData(TransactionType.SetLost, StorageState.Lost, ValueState.Settled, StorageState.Lost, ValueState.Settled)]
    [InlineData(TransactionType.SetLost, StorageState.NotAccepted, ValueState.Settled, StorageState.Lost, ValueState.Settled)]
    [InlineData(TransactionType.SetLost, StorageState.Sold, ValueState.Settled, StorageState.Lost, ValueState.Settled)]

    [InlineData(TransactionType.Settlement, StorageState.Available, ValueState.NotSettled, StorageState.Available, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.Locked, ValueState.NotSettled, StorageState.Locked, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.Lost, ValueState.NotSettled, StorageState.Lost, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.NotAccepted, ValueState.NotSettled, StorageState.NotAccepted, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.Sold, ValueState.NotSettled, StorageState.Sold, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.Available, ValueState.Settled, StorageState.Available, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.Locked, ValueState.Settled, StorageState.Locked, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.Lost, ValueState.Settled, StorageState.Lost, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.NotAccepted, ValueState.Settled, StorageState.NotAccepted, ValueState.Settled)]
    [InlineData(TransactionType.Settlement, StorageState.Sold, ValueState.Settled, StorageState.Sold, ValueState.Settled)]

    [InlineData(TransactionType.Unlock, StorageState.Available, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Unlock, StorageState.Locked, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Unlock, StorageState.Lost, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Unlock, StorageState.NotAccepted, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Unlock, StorageState.Sold, ValueState.NotSettled, StorageState.Available, ValueState.NotSettled)]
    [InlineData(TransactionType.Unlock, StorageState.Available, ValueState.Settled, StorageState.Available, ValueState.Settled)]
    [InlineData(TransactionType.Unlock, StorageState.Locked, ValueState.Settled, StorageState.Available, ValueState.Settled)]
    [InlineData(TransactionType.Unlock, StorageState.Lost, ValueState.Settled, StorageState.Available, ValueState.Settled)]
    [InlineData(TransactionType.Unlock, StorageState.NotAccepted, ValueState.Settled, StorageState.Available, ValueState.Settled)]
    [InlineData(TransactionType.Unlock, StorageState.Sold, ValueState.Settled, StorageState.Available, ValueState.Settled)]
    public void StorageStateAndValueStateShouldBeSetLike(TransactionType transactionType, StorageState storageState, ValueState valueState, StorageState expectedStorageState, ValueState expectedValueState)
    {
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = valueState
        };

        product.SetState(transactionType);

        product.StorageState.Should().Be(expectedStorageState);
        product.ValueState.Should().Be(expectedValueState);
    }
}
