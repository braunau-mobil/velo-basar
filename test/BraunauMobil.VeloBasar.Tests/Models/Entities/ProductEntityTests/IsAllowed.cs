namespace BraunauMobil.VeloBasar.Tests.Models.Entities.ProductEntityTests;

public class IsAllowed
{
    [Theory]
    [InlineData(StorageState.Available, TransactionType.Lock)]
    [InlineData(StorageState.Available, TransactionType.SetLost)]
    [InlineData(StorageState.Available, TransactionType.Sale)]
    [InlineData(StorageState.Available, TransactionType.Settlement)]
    [InlineData(StorageState.Lost, TransactionType.Unlock)]
    [InlineData(StorageState.Lost, TransactionType.Settlement)]
    [InlineData(StorageState.Locked, TransactionType.Unlock)]
    [InlineData(StorageState.Locked, TransactionType.Settlement)]
    [InlineData(StorageState.NotAccepted, TransactionType.Acceptance)]
    [InlineData(StorageState.Sold, TransactionType.Cancellation)]    
    [InlineData(StorageState.Sold, TransactionType.Settlement)]
    public void Allow(StorageState storageState, TransactionType transactionType)
    {
        //  Arrange
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = ValueState.NotSettled
        };

        //  Act
        bool result = product.IsAllowed(transactionType);

        //  Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(StorageState.Available, TransactionType.Acceptance)]
    [InlineData(StorageState.Available, TransactionType.Cancellation)]
    [InlineData(StorageState.Available, TransactionType.Unlock)]
    [InlineData(StorageState.Available, TransactionType.Unsettlement)]
    [InlineData(StorageState.Lost, TransactionType.Acceptance)]
    [InlineData(StorageState.Lost, TransactionType.Cancellation)]
    [InlineData(StorageState.Lost, TransactionType.Lock)]
    [InlineData(StorageState.Lost, TransactionType.SetLost)]
    [InlineData(StorageState.Lost, TransactionType.Sale)]
    [InlineData(StorageState.Lost, TransactionType.Unsettlement)]
    [InlineData(StorageState.Locked, TransactionType.Acceptance)]
    [InlineData(StorageState.Locked, TransactionType.Cancellation)]
    [InlineData(StorageState.Locked, TransactionType.Lock)]
    [InlineData(StorageState.Locked, TransactionType.SetLost)]
    [InlineData(StorageState.Locked, TransactionType.Sale)]
    [InlineData(StorageState.Locked, TransactionType.Unsettlement)]
    [InlineData(StorageState.NotAccepted, TransactionType.Cancellation)]
    [InlineData(StorageState.NotAccepted, TransactionType.Lock)]
    [InlineData(StorageState.NotAccepted, TransactionType.SetLost)]
    [InlineData(StorageState.NotAccepted, TransactionType.Unlock)]
    [InlineData(StorageState.NotAccepted, TransactionType.Sale)]
    [InlineData(StorageState.NotAccepted, TransactionType.Settlement)]
    [InlineData(StorageState.NotAccepted, TransactionType.Unsettlement)]
    [InlineData(StorageState.Sold, TransactionType.Acceptance)]
    [InlineData(StorageState.Sold, TransactionType.SetLost)]
    [InlineData(StorageState.Sold, TransactionType.Lock)]
    [InlineData(StorageState.Sold, TransactionType.Unlock)]
    [InlineData(StorageState.Sold, TransactionType.Sale)]
    [InlineData(StorageState.Sold, TransactionType.Unsettlement)]
    public void NotSettledDisallow(StorageState storageState, TransactionType transactionType)
    {
        //  Arrange
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = ValueState.NotSettled
        };

        //  Act
        bool result = product.IsAllowed(transactionType);

        //  Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(StorageState.Available, TransactionType.Unsettlement)]
    [InlineData(StorageState.Lost, TransactionType.Unsettlement)]
    [InlineData(StorageState.Locked, TransactionType.Unsettlement)]    
    [InlineData(StorageState.Sold, TransactionType.Unsettlement)]
    public void SettledAllow(StorageState storageState, TransactionType transactionType)
    {
        //  Arrange
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = ValueState.Settled
        };

        //  Act
        bool result = product.IsAllowed(transactionType);

        //  Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(StorageState.Available, TransactionType.Acceptance)]
    [InlineData(StorageState.Available, TransactionType.Cancellation)]
    [InlineData(StorageState.Available, TransactionType.Lock)]
    [InlineData(StorageState.Available, TransactionType.SetLost)]
    [InlineData(StorageState.Available, TransactionType.Unlock)]
    [InlineData(StorageState.Available, TransactionType.Sale)]
    [InlineData(StorageState.Available, TransactionType.Settlement)]
    [InlineData(StorageState.Lost, TransactionType.Acceptance)]
    [InlineData(StorageState.Lost, TransactionType.Cancellation)]
    [InlineData(StorageState.Lost, TransactionType.Lock)]
    [InlineData(StorageState.Lost, TransactionType.SetLost)]
    [InlineData(StorageState.Lost, TransactionType.Unlock)]
    [InlineData(StorageState.Lost, TransactionType.Sale)]
    [InlineData(StorageState.Lost, TransactionType.Settlement)]
    [InlineData(StorageState.Locked, TransactionType.Acceptance)]
    [InlineData(StorageState.Locked, TransactionType.Cancellation)]
    [InlineData(StorageState.Locked, TransactionType.Lock)]
    [InlineData(StorageState.Locked, TransactionType.SetLost)]
    [InlineData(StorageState.Locked, TransactionType.Unlock)]
    [InlineData(StorageState.Locked, TransactionType.Sale)]
    [InlineData(StorageState.Locked, TransactionType.Settlement)]
    [InlineData(StorageState.NotAccepted, TransactionType.Acceptance)]
    [InlineData(StorageState.NotAccepted, TransactionType.Cancellation)]
    [InlineData(StorageState.NotAccepted, TransactionType.Lock)]
    [InlineData(StorageState.NotAccepted, TransactionType.SetLost)]
    [InlineData(StorageState.NotAccepted, TransactionType.Unlock)]
    [InlineData(StorageState.NotAccepted, TransactionType.Sale)]
    [InlineData(StorageState.NotAccepted, TransactionType.Settlement)]
    [InlineData(StorageState.NotAccepted, TransactionType.Unsettlement)]
    [InlineData(StorageState.Sold, TransactionType.Acceptance)]
    [InlineData(StorageState.Sold, TransactionType.Cancellation)]
    [InlineData(StorageState.Sold, TransactionType.Lock)]
    [InlineData(StorageState.Sold, TransactionType.SetLost)]
    [InlineData(StorageState.Sold, TransactionType.Unlock)]
    [InlineData(StorageState.Sold, TransactionType.Sale)]
    [InlineData(StorageState.Sold, TransactionType.Settlement)]
    public void SettledDisallow(StorageState storageState, TransactionType transactionType)
    {
        //  Arrange
        ProductEntity product = new()
        {
            StorageState = storageState,
            ValueState = ValueState.Settled
        };

        //  Act
        bool result = product.IsAllowed(transactionType);

        //  Assert
        result.Should().BeFalse();
    }
}
