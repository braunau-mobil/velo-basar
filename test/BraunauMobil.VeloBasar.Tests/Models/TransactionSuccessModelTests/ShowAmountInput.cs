namespace BraunauMobil.VeloBasar.Tests.Models.TransactionSuccessModelTests;

public class ShowAmountInput
{
    [Theory]
    [VeloInlineAutoData(TransactionType.Sale)]
    [VeloInlineAutoData(TransactionType.Unsettlement)]
    public void ShouldBeTrue(TransactionType type, TransactionEntity transaction)
    {
        //  Arrange
        transaction.Type = type;

        //  Act
        TransactionSuccessModel sut = new(transaction);

        //  Assert
        sut.ShowAmountInput.Should().BeTrue();
    }

    [Fact]
    public void NoSale_ShouldBeFalse()
    {
        //  Arrange
        VeloFixture fixture = new();
        fixture.ExcludeEnumValues(TransactionType.Sale);
        fixture.ExcludeEnumValues(TransactionType.Unsettlement);
        TransactionEntity sale = fixture.Create<TransactionEntity>();

        //  Act
        TransactionSuccessModel sut = new(sale);

        //  Assert
        sut.ShowAmountInput.Should().BeFalse();
    }
}
