namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class CanCancel
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Acceptance, false)]
    [VeloInlineAutoData(TransactionType.Cancellation, false)]
    [VeloInlineAutoData(TransactionType.Lock, false)]
    [VeloInlineAutoData(TransactionType.Sale, true)]
    [VeloInlineAutoData(TransactionType.SetLost, false)]
    [VeloInlineAutoData(TransactionType.Settlement, false)]
    [VeloInlineAutoData(TransactionType.Unlock, false)]
    public void ShouldBe(TransactionType type, bool expectedResult, TransactionEntity sut)
    {
        //  Arrange
        sut.Type = type;

        //  Act
        bool result = sut.CanCancel;

        //  Assert
        result.Should().Be(expectedResult);
    }
}
