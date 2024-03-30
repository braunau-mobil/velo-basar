namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class CanHasDocument
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Acceptance, true)]
    [VeloInlineAutoData(TransactionType.Cancellation, false)]
    [VeloInlineAutoData(TransactionType.Lock, false)]
    [VeloInlineAutoData(TransactionType.Sale, true)]
    [VeloInlineAutoData(TransactionType.SetLost, false)]
    [VeloInlineAutoData(TransactionType.Settlement, true)]
    [VeloInlineAutoData(TransactionType.Unlock, false)]
    [VeloInlineAutoData(TransactionType.Unsettlement, false)]
    public void ShouldBe(TransactionType type, bool expectedResult, TransactionEntity sut)
    {
        //  Arrange
        sut.Type = type;

        //  Act
        bool result = sut.CanHasDocument;

        //  Assert
        result.Should().Be(expectedResult);
    }
}
