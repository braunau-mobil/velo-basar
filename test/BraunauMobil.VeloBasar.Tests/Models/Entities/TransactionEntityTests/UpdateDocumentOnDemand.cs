namespace BraunauMobil.VeloBasar.Tests.Models.Entities.TransactionEntityTests;

public class UpdateDocumentOnDemand
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Acceptance, true)]
    [VeloInlineAutoData(TransactionType.Cancellation, false)]
    [VeloInlineAutoData(TransactionType.Lock, false)]
    [VeloInlineAutoData(TransactionType.Sale, false)]
    [VeloInlineAutoData(TransactionType.SetLost, false)]
    [VeloInlineAutoData(TransactionType.Settlement, false)]
    [VeloInlineAutoData(TransactionType.Unlock, false)]
    public void ShouldBe(TransactionType type, bool expectedResult, TransactionEntity sut)
    {
        //  Arrange
        sut.Type = type;

        //  Act
        bool result = sut.UpdateDocumentOnDemand;

        //  Assert
        result.Should().Be(expectedResult);
    }
}
