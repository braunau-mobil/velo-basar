namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class NeedsStatusPush
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Acceptance, true)]
    [VeloInlineAutoData(TransactionType.Cancellation, true)]
    [VeloInlineAutoData(TransactionType.Lock, false)]
    [VeloInlineAutoData(TransactionType.Sale, true)]
    [VeloInlineAutoData(TransactionType.SetLost, false)]
    [VeloInlineAutoData(TransactionType.Settlement, true)]
    [VeloInlineAutoData(TransactionType.Unlock, false)]
    public void ShouldBe(TransactionType type, bool expectedResult, TransactionEntity sut)
    {
        //  Arrange
        sut.Type = type;

        //  Act
        bool result = sut.NeedsStatusPush;

        //  Assert
        result.Should().Be(expectedResult);
    }
}
