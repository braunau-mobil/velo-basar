namespace BraunauMobil.VeloBasar.Tests.Models.Entities.SellerEntityTests;

public class UpdateValueState
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Settlement)]
    public void ShouldSetToSettled(TransactionType transactionType, SellerEntity sut)
    {
        //  Arrange
        sut.ValueState = ValueState.NotSettled;

        //  Act
        sut.UpdateValueState(transactionType);

        //  Assert
        sut.ValueState.Should().Be(ValueState.Settled);
    }

    [Theory]
    [VeloInlineAutoData(TransactionType.Acceptance)]
    [VeloInlineAutoData(TransactionType.Unsettlement)]
    public void ShouldSetToNotSettled(TransactionType transactionType, SellerEntity sut)
    {
        //  Arrange
        sut.ValueState = ValueState.Settled;

        //  Act
        sut.UpdateValueState(transactionType);

        //  Assert
        sut.ValueState.Should().Be(ValueState.NotSettled);
    }

    [Theory]
    [VeloInlineAutoData(TransactionType.Cancellation)]
    [VeloInlineAutoData(TransactionType.Lock)]
    [VeloInlineAutoData(TransactionType.Sale)]
    [VeloInlineAutoData(TransactionType.SetLost)]
    [VeloInlineAutoData(TransactionType.Unlock)]
    public void ShouldNotSetAtAll(TransactionType transactionType, SellerEntity sut)
    {
        //  Arrange
        ValueState before = sut.ValueState;

        //  Act
        sut.UpdateValueState(transactionType);

        //  Assert
        sut.ValueState.Should().Be(before);
    }
}
